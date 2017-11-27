using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace API.CRUD.Controllers
{
    [Route("test")]
    public class TestController : ApiController
    {
        [HttpGet("error")]
        public IActionResult Error()
        {
            throw new Exception("Testing errors");
        }

        [HttpGet("validationError")]
        public IActionResult ValidationError()
        {
            throw new ValidationException("Some validation error like: age is not valid.");
        }

        [HttpGet("validationErrors")]
        public IActionResult ValidationErrors()
        {
            var errors = new List<ValidationFailure>
            {
                new ValidationFailure("age", "Age is not valid."),
                new ValidationFailure("id", "no person Id was provided."),
                new ValidationFailure("name", "No person name was provided.")
            };
            
            throw new ValidationException(errors);
        }
    }
}