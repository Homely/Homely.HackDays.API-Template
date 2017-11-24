using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace API.CRUD.Extensions
{
    public static class JsonStatusCodePagesExtensions
    {
        private const string JsonContentType = "application/json";

        /// <summary>
        /// Status code pages are rendered as JSON compatible responses.
        /// </summary>
        /// <param name="app">A class that provides the mechanisms to configure an application's request.</param>
        /// <returns>This class that provides the mechanisms to configure an application's request</returns>
        public static IApplicationBuilder JsonStatusCodePages(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseStatusCodePages(OnStatusCodePagesAsync);
        }

        private static Task OnStatusCodePagesAsync(StatusCodeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.HttpContext.Response.ContentType = JsonContentType;

            // TODO: Replace with the ApiError model?
            return context.HttpContext.Response.WriteAsync($"{{\"statusCode\": {context.HttpContext.Response.StatusCode}}}");
        }
    }
}