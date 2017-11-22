using Microsoft.AspNetCore.Mvc;
using Xunit;

// ReSharper disable InconsistentNaming

namespace API.CRUD.UnitTests.Helpers
{
    public static class IActionResultExtensions
    {
        public static void ShouldBeOkWithModel<T>(this IActionResult result,
                                                  T expectedModel) where T : class
        {
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var model = okResult.Value as T;
            Assert.NotNull(model);
            model.ShouldLookLike(expectedModel);
        }

        public static void ShouldBeNotFound(this IActionResult result)
        {
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundResult;
            Assert.NotNull(notFoundResult);
        }

        public static void ShouldBeBadRequest(this IActionResult result)
        {
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
        }

        public static void ShouldBeCreated(this IActionResult result)
        {
            Assert.NotNull(result);
            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
        }
    }
}
