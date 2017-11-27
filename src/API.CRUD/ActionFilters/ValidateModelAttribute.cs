using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.CRUD.ActionFilters
{
    // http://www.jerriepelser.com/blog/validation-response-aspnet-core-webapi/
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var modelStateErrors = new Dictionary<string, string>();
                foreach (var error in context.ModelState)
                {
                    foreach (var keyValue in error.Value.Errors)
                    {
                        modelStateErrors.Add(error.Key, keyValue.ErrorMessage);
                    }
                }

                //var modelStateErrors = context.ModelState.ToDictionary(key => key.Key, value => value.Value);
                var apiErrors = new ApiError(modelStateErrors);
                context.Result = new BadRequestObjectResult(apiErrors);
            }
        }
    }
}