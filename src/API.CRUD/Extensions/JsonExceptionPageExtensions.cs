using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace API.CRUD.Extensions
{
    public static class JsonExceptionPageExtensions
    {
        private const string JsonContentType = "application/json";

        /// <summary>
        /// Adds a simple json error message for a WebApi/REST website.
        /// </summary>
        /// <param name="app">A class that provides the mechanisms to configure an application's request.</param>
        /// <returns>This class that provides the mechanisms to configure an application's request</returns>
        public static IApplicationBuilder JsonExceptionPage(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseExceptionHandler(options => options.Run(
                async httpContext => await ExceptionResponseAsync(httpContext, true)));
        }

        private static async Task ExceptionResponseAsync(HttpContext httpContext, bool isDevelopmentEnvironment)
        {
            // Re-Apply CORS headers because they have been 'cleared'.
            // REF: https://stackoverflow.com/questions/47225012/how-to-send-an-http-4xx-5xx-response-with-cors-headers-in-an-aspnet-core-web-app/47232876
            var corsService = httpContext.RequestServices.GetService(typeof(ICorsService)) as ICorsService;
            var corsPolicyProvider = httpContext.RequestServices.GetService(typeof(ICorsPolicyProvider)) as ICorsPolicyProvider;
            var corsPolicy = await corsPolicyProvider.GetPolicyAsync(httpContext, ServiceCollectionExtensions.CorsPolicyName);
            corsService.ApplyResult(corsService.EvaluatePolicy(httpContext, corsPolicy),
                                    httpContext.Response);
            
            var exceptionFeature = httpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionFeature == null)
            {
                // An unknow and unhandled exception occured. So this is like a fallback.
                exceptionFeature = new ExceptionHandlerFeature
                {
                    Error = new Exception("An unhandled and unexpected error has occured. Ro-roh :~(.")
                };
            }

            await ConvertExceptionToJsonResponseAsync(exceptionFeature,
                                                      httpContext.Response, 
                                                      isDevelopmentEnvironment);
        }

        private static Task ConvertExceptionToJsonResponseAsync(IExceptionHandlerPathFeature exceptionFeature,
            HttpResponse response,
            bool isDevelopmentEnvironment)
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
            var includeStackTrace = false;
            var statusCode = HttpStatusCode.InternalServerError;
            var error = new ApiError();

            if (exception is ValidationException)
            {
                statusCode = HttpStatusCode.BadRequest;
                foreach(var validationError in ((ValidationException)exception).Errors)
                {
                    error.AddError(validationError.PropertyName, validationError.ErrorMessage);
                }
            }
            else
            {
                // Final fallback.
                includeStackTrace = true;
                error.AddError(exception.Message);
            }

            if (includeStackTrace &&
                isDevelopmentEnvironment)
            {
                error.StackTrace = exception.StackTrace;
            }

            var json = JsonConvert.SerializeObject(error, JsonHelpers.JsonSerializerSettings);
            response.StatusCode = (int)statusCode;
            response.ContentType = JsonContentType;
            return response.WriteAsync(json);
        }    
    }
}