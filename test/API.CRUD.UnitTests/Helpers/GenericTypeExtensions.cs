using Newtonsoft.Json;
using Xunit;

namespace API.CRUD.UnitTests.Helpers
{
    public static class GenericTypeExtensions
    {
        public static void ShouldLookLike<T>(this T actual,
                                             T expected)
        {
            Assert.NotNull(actual);
            Assert.NotNull(expected);
            var actualJson = JsonConvert.SerializeObject(actual);
            var expectedJson = JsonConvert.SerializeObject(expected);
            Assert.Equal(expectedJson, actualJson);
        }
    }
}
