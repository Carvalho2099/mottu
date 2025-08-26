using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace VehicleManagement.Api.Services
{
    public class MinioService
    {
        private readonly IMinioClient _minioClient;
        private readonly string _bucketName;
        private readonly ILogger<MinioService> _logger;

        public MinioService(IConfiguration configuration, ILogger<MinioService> logger)
        {
            _bucketName = configuration["Minio:Bucket"] ?? "vehicle-files";
            _minioClient = new MinioClient()
                .WithEndpoint(configuration["Minio:Endpoint"] ?? "localhost:9000")
                .WithCredentials(configuration["Minio:AccessKey"], configuration["Minio:SecretKey"])
                .WithSSL(false)
                .Build();
            _logger = logger;
        }

        // Construtor para testes
        public MinioService(IConfiguration configuration, IMinioClient minioClient, ILogger<MinioService> logger)
        {
            _bucketName = configuration["Minio:Bucket"] ?? "vehicle-files";
            _minioClient = minioClient;
            _logger = logger;
        }

        public async Task<string> GetPresignedUploadUrlAsync(string objectName, int expirySeconds = 3600)
        {
            try
            {
                await EnsureBucketExistsAsync();
                var args = new PresignedPutObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(objectName)
                    .WithExpiry(expirySeconds);
                var url = await _minioClient.PresignedPutObjectAsync(args);
                _logger.LogInformation($"Presigned upload URL generated for object {objectName} with expiry {expirySeconds} seconds.");
                return url;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error generating presigned upload URL for object {objectName}.");
                throw;
            }
        }

        public async Task<List<string>> ListFilesAsync(string prefix)
        {
            await EnsureBucketExistsAsync();
            var files = new List<string>();
            var args = new ListObjectsArgs()
                .WithBucket(_bucketName)
                .WithPrefix(prefix)
                .WithRecursive(true);
            await foreach (var item in _minioClient.ListObjectsEnumAsync(args))
            {
                files.Add(item.Key);
            }
            return files;
        }

        private async Task EnsureBucketExistsAsync()
        {
            var existsArgs = new BucketExistsArgs().WithBucket(_bucketName);
            bool found = await _minioClient.BucketExistsAsync(existsArgs);
            if (!found)
            {
                var makeArgs = new MakeBucketArgs().WithBucket(_bucketName);
                await _minioClient.MakeBucketAsync(makeArgs);
            }
        }
    }
}
