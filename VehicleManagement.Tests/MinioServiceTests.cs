using Xunit;
using Moq;
using Minio;
using VehicleManagement.Api.Services;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace VehicleManagement.Tests
{
    public class MinioServiceTests
    {
        [Fact]
        public async Task GetPresignedUploadUrlAsync_ReturnsUrl()
        {
            var mockConfig = new Mock<IConfiguration>();
mockConfig.Setup(c => c["Minio__Endpoint"]).Returns("localhost");
mockConfig.Setup(c => c["Minio__AccessKey"]).Returns("key");
mockConfig.Setup(c => c["Minio__SecretKey"]).Returns("secret");
mockConfig.Setup(c => c["Minio__Bucket"]).Returns("bucket");

var mockMinioClient = new Mock<IMinioClient>();
mockMinioClient.Setup(m => m.BucketExistsAsync(It.IsAny<Minio.DataModel.Args.BucketExistsArgs>(), default))
    .ReturnsAsync(true);
mockMinioClient.Setup(m => m.PresignedPutObjectAsync(It.IsAny<Minio.DataModel.Args.PresignedPutObjectArgs>()))
    .ReturnsAsync("http://mocked-url");

var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<MinioService>>();
var service = new MinioService(mockConfig.Object, mockMinioClient.Object, mockLogger.Object);
var url = await service.GetPresignedUploadUrlAsync("file.txt");
Assert.NotNull(url);
Assert.Equal("http://mocked-url", url);
        }
    }
}
