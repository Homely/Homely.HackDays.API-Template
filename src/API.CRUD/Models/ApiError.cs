using System;
using System.Collections.Generic;

namespace API.CRUD
{
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

        public void AddError(IDictionary<string, string> errorMessages)
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
    }
}