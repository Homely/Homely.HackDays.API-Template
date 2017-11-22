using Microsoft.AspNetCore.Mvc;

namespace API.CRUD.UnitTests.Helpers
{
    public static class ControllerBaseExtensions
    {
        /// <summary>
        /// Used to test bad request responses. Error doesn't matter.
        /// </summary>
        /// <param name="controller"></param>
        public static void SetInvalidModelState(this ControllerBase controller)
        {
            controller.ModelState.AddModelError("x", "x");
        }
    }
}
