using Xunit;
using Moq;
using VehicleManagement.Api.Services;
using Microsoft.Extensions.Configuration;

namespace VehicleManagement.Tests
{
    public class RabbitMqServiceTests
    {
        [Fact]
        public void Constructor_InitializesConnection()
        {
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["RabbitMQ__Host"]).Returns("localhost");
            mockConfig.Setup(c => c["RabbitMQ__User"]).Returns("guest");
            mockConfig.Setup(c => c["RabbitMQ__Password"]).Returns("guest");

            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<RabbitMqService>>();
            var service = new RabbitMqService(mockConfig.Object, mockLogger.Object);
            Assert.NotNull(service);
        }
    }
}
