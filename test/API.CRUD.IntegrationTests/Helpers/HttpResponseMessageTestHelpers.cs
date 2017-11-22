using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace API.CRUD.IntegrationTests.Helpers
{
    public static class HttpResponseMessageTestHelpers
    {
        public static async Task EnsureSuccessfulJsonResponse<T>(this HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseString);
            var model = JsonConvert.DeserializeObject<T>(responseString);
            Assert.NotNull(model);
        }
    }
}