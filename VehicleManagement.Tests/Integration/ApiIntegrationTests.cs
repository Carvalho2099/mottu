using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using VehicleManagement.Api;

namespace VehicleManagement.Tests.Integration
{
    public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_HealthCheck_ReturnsOk()
        {
            var response = await _client.GetAsync("/health");
            response.EnsureSuccessStatusCode();
        }
    }
}
