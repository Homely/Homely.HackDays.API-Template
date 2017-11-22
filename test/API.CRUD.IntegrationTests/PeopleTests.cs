using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using API.CRUD.IntegrationTests.Helpers;
using API.CRUD.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;
// ReSharper disable InvokeAsExtensionMethod

namespace API.CRUD.IntegrationTests
{
    public class PeopleTests
    {
        private readonly HttpClient _client;

        public PeopleTests()
        {
            // Arrange.
            _client = new TestServer(new WebHostBuilder()
                                         .UseStartup<Startup>()).CreateClient();
        }

        [Fact]
        public async Task DoingAGetRequestToPeople_ShouldReturnPeopleAsJson()
        {
            // Act.
            var response = await _client.GetAsync("/api/people");

            // Assert.
            await HttpResponseMessageTestHelpers.EnsureSuccessfulJsonResponse<IEnumerable<Person>>(response);
        }

        [Fact]
        public async Task DoingAGetRequestToPerson_ShouldReturnAPersonAsJson()
        {
            // Act.
            var response = await _client.GetAsync("/api/people/1");

            // Assert.
            await HttpResponseMessageTestHelpers.EnsureSuccessfulJsonResponse<Person>(response);
        }
    }
}