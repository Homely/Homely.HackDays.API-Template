using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.CRUD.Extensions
{
    public static class ApiErrorExtensions
    {
        public static IEnumerable<ApiError> ToApiErrors(this IDictionary<string, string> errorMessages)
        {
            if (errorMessages == null)
            {
                throw new ArgumentNullException(nameof(errorMessages));
            }

            return errorMessages.Select(error => new ApiError
            {
                Key = error.Key,
                Message = error.Value
            });
        }

        public static IEnumerable<ApiError> ToApiErrors(this ValidationException validationException)
        {
            // We either have a collection of errors 
            //  - or -
            // We just have an error message.
            if (validationException.Errors != null &&
                validationException.Errors. Any())
            {
                var errors = validationException.Errors.ToDictionary(key => key.PropertyName, value => value.ErrorMessage);
                return errors.ToApiErrors();
            }
            else
            {
                return new [] { new ApiError(validationException.Message) };
            }
        }
    }
}
