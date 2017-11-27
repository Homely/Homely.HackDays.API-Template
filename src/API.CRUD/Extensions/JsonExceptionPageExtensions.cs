using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace API.CRUD.Extensions
{
    public static class JsonExceptionPageExtensions
    {
        private const string JsonContentType = "application/json";

        /// <summary>
        /// Adds a simple json error message for a WebApi/REST website.
        /// </summary>
        /// <param name="app">A class that provides the mechanisms to configure an application's request.</param>
        /// <param name="corsPolicyName">Name of the CORS policy to re-add to the specific details back to the response header.</param>
        /// <returns>This class that provides the mechanisms to configure an application's request</returns>
        public static IApplicationBuilder JsonExceptionPage(this IApplicationBuilder app,
                                                            string corsPolicyName)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (string.IsNullOrWhiteSpace(corsPolicyName))
            {
                throw new ArgumentNullException(nameof(corsPolicyName));
            }

            return app.UseExceptionHandler(options => options.Run(
                                               async httpContext => await ExceptionResponseAsync(httpContext, corsPolicyName)));
        }

        private static async Task ExceptionResponseAsync(HttpContext httpContext,
                                                         string corsPolicyName)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (string.IsNullOrWhiteSpace(corsPolicyName))
            {
                throw new ArgumentException(nameof(corsPolicyName));
            }

            await ReApplyCorsPolicyToHeaderAsync(httpContext, corsPolicyName).ConfigureAwait(false);

            var exceptionFeature = httpContext.Features.Get<IExceptionHandlerPathFeature>() ?? new ExceptionHandlerFeature
            {
                Error = new Exception("An unhandled and unexpected error has occured. Ro-roh :~(.")
            };

            await ConvertExceptionToJsonResponseAsync(exceptionFeature,
                                                      httpContext.Response);
        }

        /// The headers are cleaered when a custom exception is applied. 
        /// REF: https://github.com/aspnet/HttpAbstractions/blob/dev/src/Microsoft.AspNetCore.Http.Extensions/ResponseExtensions.cs#L19
        ///      https://stackoverflow.com/questions/47225012/how-to-send-an-http-4xx-5xx-response-with-cors-headers-in-an-aspnet-core-web-app/47232876?noredirect=1#comment81470664_47232876
        /// It's a PITA :( As such, we need to re-apply the named CORS policy to the response headers.
        private static async Task ReApplyCorsPolicyToHeaderAsync(HttpContext httpContext,
                                                                 string corsPolicyName)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (string.IsNullOrWhiteSpace(corsPolicyName))
            {
                throw new ArgumentException(nameof(corsPolicyName));
            }

            // REF: https://stackoverflow.com/questions/47225012/how-to-send-an-http-4xx-5xx-response-with-cors-headers-in-an-aspnet-core-web-app/47232876
            if (!(httpContext.RequestServices.GetService(typeof(ICorsService)) is ICorsService corsService))
            {
                return;
            }

            if (!(httpContext.RequestServices.GetService(typeof(ICorsPolicyProvider)) is ICorsPolicyProvider corsPolicyProvider))
            {
                // We've failed to retrieve the named CORS policy and as such, can't add any headers back to the response :(
                return;
            }

            var corsPolicy = await corsPolicyProvider.GetPolicyAsync(httpContext, corsPolicyName);
            corsService.ApplyResult(corsService.EvaluatePolicy(httpContext, corsPolicy),
                                    httpContext.Response);
        }

        private static Task ConvertExceptionToJsonResponseAsync(IExceptionHandlerPathFeature exceptionFeature,
                                                                HttpResponse response)
        {
            if (exceptionFeature == null)
            {
                throw new ArgumentNullException(nameof(exceptionFeature));
            }

            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            var exception = exceptionFeature.Error;
            var statusCode = HttpStatusCode.InternalServerError;
            var apiError = new ApiError();

            if (exception is ValidationException validationException)
            {
                statusCode = HttpStatusCode.BadRequest;

                // We either have a collection of errors 
                //  - or -
                // We just have an error message.
                if (validationException.Errors != null &&
                    validationException.Errors.Any())
                {
                    var errors = validationException.Errors.ToDictionary(key => key.PropertyName, value => value.ErrorMessage);
                    apiError.AddError(errors);
                }
                else
                {
                    apiError.AddError(validationException.Message);
                }
            }
            else
            {
                // Final fallback - any/all other errors.
                apiError.AddError(exception.Message);
            }

            var json = JsonConvert.SerializeObject(apiError, JsonHelpers.JsonSerializerSettings);
            response.StatusCode = (int) statusCode;
            response.ContentType = JsonContentType;
            return response.WriteAsync(json);
        }
    }
}