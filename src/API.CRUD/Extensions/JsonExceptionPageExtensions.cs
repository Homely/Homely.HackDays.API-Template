using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace API.CRUD.Extensions
{
    public static class JsonExceptionPageExtensions
    {
        private const string JsonContentType = "application/json";

        /// <summary>
        /// Adds a simple json error message for a WebApi/REST website.
        /// </summary>
        /// <param name="app">A class that provides the mechanisms to configure an application's request.</param>
        /// <remarks>This doesn't handle any CORS requests. So there's no CORS headers in this RESPONSE payload.</remarks>
        /// <returns>This class that provides the mechanisms to configure an application's request</returns>
        public static IApplicationBuilder JsonExceptionPage(this IApplicationBuilder app,
                                                            bool includeStackTrace)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            
            return app.UseExceptionHandler(options => options.Run(
                                               async httpContext => await ExceptionResponseAsync(httpContext, includeStackTrace)));
        }

        private static Task ExceptionResponseAsync(HttpContext httpContext,
                                                   bool includeStackTrace)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }
            
            var exceptionFeature = httpContext.Features.Get<IExceptionHandlerPathFeature>() ?? new ExceptionHandlerFeature
            {
                Error = new Exception("An unhandled and unexpected error has occured while trying retrieve the ExceptionHandlerPathFeature. Ro-roh :~(.")
            };
            
            var exception = exceptionFeature.Error;
            var statusCode = HttpStatusCode.InternalServerError;
            
            IEnumerable<ApiError> apiErrors;
            if (exception is ValidationException validationException)
            {
                statusCode = HttpStatusCode.BadRequest;
                apiErrors = validationException.ToApiErrors();
            }
            else
            {
                // Final fallback - any/all other errors.
                apiErrors = new [] { new ApiError(exception.Message)};
            }

            // Prepare the actual RESPONSE payload.
            httpContext.Response.StatusCode = (int) statusCode;
            httpContext.Response.ContentType = JsonContentType;
            
            return httpContext.Response.WriteApiErrorsAsJsonAsync(apiErrors,
                                                                  includeStackTrace 
                                                                      ? exception.StackTrace
                                                                      : null);
        }
    }
}