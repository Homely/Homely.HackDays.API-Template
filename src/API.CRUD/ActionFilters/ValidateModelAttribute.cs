using System.Collections.Generic;
using API.CRUD.Extensions;
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

                var apiErrors = modelStateErrors.ToApiErrors();
                context.Result = new BadRequestObjectResult(apiErrors);
            }
        }
    }
}