using Xunit;
using Moq;
using VehicleManagement.Api.Services;
using VehicleManagement.Api.Models;
using VehicleManagement.Api.Repositories.Interfaces;
using System.Threading.Tasks;
using VehicleManagement.Api.Services.Interfaces;

namespace VehicleManagement.Tests
{
    public class VehicleServiceTests
    {
        [Fact]
        public async Task GetVehiclesAsync_ReturnsList()
        {
            var mockRepo = new Mock<IVehicleRepository>();
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Vehicle> { new Vehicle { Id = "1", Model = "Model S", Plate = "XYZ9876", Year = 2022 } });
            var mockQueue = new Mock<IMessageQueueService>();
            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<VehicleService>>();
            var service = new VehicleService(mockRepo.Object, mockQueue.Object, mockLogger.Object);

            var result = await service.GetVehiclesAsync(null, null);
            Assert.Single(result);
        }
    }
}
