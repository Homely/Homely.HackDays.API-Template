using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.CRUD
{
    public class ApiError
    {
        public ApiError()
        {   
        }

        public ApiError(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentException(nameof(errorMessage));
            }
            
            Message = errorMessage;
        }

        public string Key { get; set; }

        public string Message { get; set; }
    }
    /*
    public class ApiError
    {
        private const string DefaultErrorKey = "Error";

        public ApiError()
        {
            Errors = new Dictionary<string, string>();
        }

        public ApiError(string errorMessage) : this()
        {
            AddError(errorMessage);
        }

        public ApiError(IDictionary<string, string> errorMessages)
        {
            Errors = errorMessages;
        }

        public IDictionary<string, string> Errors { get; }
        public string StackTrace { get; set; }

        public void AddError(string errorMessage)
        {
            AddError(DefaultErrorKey, errorMessage);
        }

        public void AddError(string key,
                             string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException(nameof(key));
            }

            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentException(nameof(errorMessage));
            }

            if (Errors.ContainsKey(DefaultErrorKey))
            {
                Errors[key] = $"{Errors[key]} {errorMessage}";
            }
            else
            {
                Errors[key] = errorMessage;
            }
        }

        public void AddErrors(IDictionary<string, string> errorMessages)
        {
            if (errorMessages == null)
            {
                throw new ArgumentNullException(nameof(errorMessages));
            }

            foreach (var error in errorMessages)
            {
                AddError(error.Key, error.Value);
            }
        }
        
        public void AddErrors(ValidationException validationException)
        {
            // We either have a collection of errors 
            //  - or -
            // We just have an error message.
            if (validationException.Errors != null &&
                validationException.Errors. Any())
            {
                var errors = validationException.Errors.ToDictionary(key => key.PropertyName, value => value.ErrorMessage);
                AddErrors(errors);
            }
            else
            {
                AddError(validationException.Message);
            }
        }

    }

    */
}