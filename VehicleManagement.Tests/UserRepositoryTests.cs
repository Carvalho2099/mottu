using Xunit;
using Microsoft.Extensions.Configuration;
using Moq;
using VehicleManagement.Api.Repositories;
using VehicleManagement.Api.Models;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace VehicleManagement.Tests
{
    public class UserRepositoryTests
    {
        [Fact]
        public async Task FindByUsernameAsync_ReturnsNull_WhenUserNotFound()
        {
            var users = new List<User>();
var mockCursor = new Mock<IAsyncCursor<User>>();
mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<System.Threading.CancellationToken>()))
    .Returns(true)
    .Returns(false);
mockCursor.SetupSequence(x => x.MoveNextAsync(It.IsAny<System.Threading.CancellationToken>()))
    .ReturnsAsync(true)
    .ReturnsAsync(false);
mockCursor.Setup(x => x.Current).Returns(users);

var mockCollection = new Mock<IMongoCollection<User>>();
mockCollection.Setup(c => c.FindAsync(
    It.IsAny<FilterDefinition<User>>(),
    It.IsAny<FindOptions<User>>(),
    It.IsAny<System.Threading.CancellationToken>()
)).ReturnsAsync(mockCursor.Object);

var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<UserRepository>>();
var repo = new UserRepository(mockCollection.Object, mockLogger.Object);
var result = await repo.GetByUsernameAsync("notfound");
Assert.Null(result);
        }
    }
}
