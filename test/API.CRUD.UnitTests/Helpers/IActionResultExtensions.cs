using Microsoft.AspNetCore.Mvc;
using Xunit;

// ReSharper disable InconsistentNaming

namespace API.CRUD.UnitTests.Helpers
{
    public static class IActionResultExtensions
    {
        public static IActionResult ShouldBeOk(this IActionResult result)
        {
            Assert.NotNull(result);
            var okResult = result as OkResult;
            Assert.NotNull(okResult);
            return result;
        }

        public static IActionResult WithModel<T>(this IActionResult result,
                                                 T expectedModel)
            where T : class
        {
            Assert.NotNull(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            var model = objectResult.Value as T;
            Assert.NotNull(model);
            model.ShouldLookLike(expectedModel);
            return result;
        }

        public static IActionResult ShouldBeOkWithModel<T>(this IActionResult result,
                                                           T expectedModel) where T : class
        {
            Assert.NotNull(result);
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            okObjectResult.WithModel(expectedModel);
            return result;
        }

        public static IActionResult ShouldBeNotFound(this IActionResult result)
        {
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundResult;
            Assert.NotNull(notFoundResult);
            return result;
        }

        public static IActionResult ShouldBeBadRequest(this IActionResult result)
        {
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            return result;
        }

        public static IActionResult ShouldBeCreated(this IActionResult result)
        {
            Assert.NotNull(result);
            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            return result;
        }

        public static IActionResult ShouldBeNoContent(this IActionResult result)
        {
            Assert.NotNull(result);
            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            return result;
        }
    }
}