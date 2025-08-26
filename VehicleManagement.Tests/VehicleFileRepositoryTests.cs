using Xunit;
using Microsoft.Extensions.Configuration;
using Moq;
using VehicleManagement.Api.Repositories;
using VehicleManagement.Api.Models;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace VehicleManagement.Tests
{
    public class VehicleFileRepositoryTests
    {
        [Fact]
        public async Task GetByVehicleIdAsync_ReturnsFiles()
        {
            var files = new List<VehicleFile> { new VehicleFile { Id = "file1", VehicleId = "vehicle1", FileName = "doc.pdf" } };
var mockCursor = new Mock<IAsyncCursor<VehicleFile>>();
mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<System.Threading.CancellationToken>()))
    .Returns(true)
    .Returns(false);
mockCursor.SetupSequence(x => x.MoveNextAsync(It.IsAny<System.Threading.CancellationToken>()))
    .ReturnsAsync(true)
    .ReturnsAsync(false);
mockCursor.Setup(x => x.Current).Returns(files);

var mockCollection = new Mock<IMongoCollection<VehicleFile>>();
mockCollection.Setup(c => c.FindAsync(
    It.IsAny<FilterDefinition<VehicleFile>>(),
    It.IsAny<FindOptions<VehicleFile>>(),
    It.IsAny<System.Threading.CancellationToken>()
)).ReturnsAsync(mockCursor.Object);

var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<VehicleFileRepository>>();
var repo = new VehicleFileRepository(mockCollection.Object, mockLogger.Object);
var result = await repo.GetByVehicleIdAsync("vehicle1");
Assert.NotNull(result);
Assert.Single(result);
Assert.Equal("file1", result[0].Id);
        }
    }
}
