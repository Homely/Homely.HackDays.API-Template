using System;
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
    }
}