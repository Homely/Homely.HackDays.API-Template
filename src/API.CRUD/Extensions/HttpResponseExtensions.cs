﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace API.CRUD.Extensions
{
    public static class HttpResponseExtensions
    {
        public static Task WriteApiErrorsAsJsonAsync(this HttpResponse response,
                                                     ApiError apiError, 
                                                     string stackTrace = null,
                                                     CancellationToken cancellationToken = default(CancellationToken))
        {
            return response.WriteApiErrorsAsJsonAsync(new [] { apiError }, 
                                                      stackTrace,
                                                      cancellationToken);
        }

        public static Task WriteApiErrorsAsJsonAsync(this HttpResponse response,
                                                     IEnumerable<ApiError> apiErrors, 
                                                     string stackTrace = null,
                                                     CancellationToken cancellationToken = default(CancellationToken))
        {
            /* Errors result will look like:
               {
                   "errors": [
                        {
                            ... <api error in here> ...
                        },
                        {
                            ... <api error in here> ...
                        }
                   ]
               }
            */
            var errorModel = new
            {
                errors = apiErrors,
                stacktrace = stackTrace
            };

            var json = JsonConvert.SerializeObject(errorModel, JsonHelpers.JsonSerializerSettings);
            
            // Send that RESPONSE payload!
            return response.WriteAsync(json, cancellationToken);
        }
    }
}
