using Xunit;
using Microsoft.Extensions.Configuration;
using Moq;
using VehicleManagement.Api.Repositories;
using VehicleManagement.Api.Models;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace VehicleManagement.Tests
{
    public class VehicleRepositoryTests
    {
        [Fact]
        public async Task GetAllAsync_ReturnsVehicles()
        {
            var vehicles = new List<Vehicle> { new Vehicle { Id = "1", Model = "Model S", Plate = "XYZ9876", Year = 2022 } };
var mockCursor = new Mock<IAsyncCursor<Vehicle>>();
mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<System.Threading.CancellationToken>()))
    .Returns(true)
    .Returns(false);
mockCursor.SetupSequence(x => x.MoveNextAsync(It.IsAny<System.Threading.CancellationToken>()))
    .ReturnsAsync(true)
    .ReturnsAsync(false);
mockCursor.Setup(x => x.Current).Returns(vehicles);

var mockCollection = new Mock<IMongoCollection<Vehicle>>();
mockCollection.Setup(c => c.FindAsync(
    It.IsAny<FilterDefinition<Vehicle>>(),
    It.IsAny<FindOptions<Vehicle>>(),
    It.IsAny<System.Threading.CancellationToken>()
)).ReturnsAsync(mockCursor.Object);

var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<VehicleRepository>>();
var repo = new VehicleRepository(mockCollection.Object, mockLogger.Object);
var result = await repo.GetAllAsync();
Assert.NotNull(result);
Assert.Single(result);
Assert.Equal("1", result[0].Id);
        }
    }
}
