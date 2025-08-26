using MongoDB.Driver;
using VehicleManagement.Api.Models;
using Microsoft.Extensions.Configuration;

using VehicleManagement.Api.Repositories.Interfaces;

namespace VehicleManagement.Api.Repositories
{
    public class VehicleFileRepository : IVehicleFileRepository
    {
        private readonly IMongoCollection<VehicleFile> _files;
        private readonly ILogger<VehicleFileRepository> _logger;

        public VehicleFileRepository(IConfiguration configuration, ILogger<VehicleFileRepository> logger)
        {
            var connStr = configuration["Mongo:ConnectionString"];
            var client = new MongoClient(connStr);
            var database = client.GetDatabase("vehicle_management");
            _files = database.GetCollection<VehicleFile>("VehicleFiles");
            _logger = logger;
        }

        // Construtor para testes
        public VehicleFileRepository(IMongoCollection<VehicleFile> filesCollection, ILogger<VehicleFileRepository> logger)
        {
            _files = filesCollection;
            _logger = logger;
        }

        public async Task<List<VehicleFile>> GetByVehicleIdAsync(string vehicleId)
        {
            _logger.LogInformation("[REPO] GetByVehicleIdAsync called for vehicleId={VehicleId}", vehicleId);
            var result = await _files.Find(f => f.VehicleId == vehicleId).ToListAsync();
            _logger.LogInformation("[REPO] GetByVehicleIdAsync returned {Count} files", result.Count);
            return result;
        }

        public async Task<VehicleFile?> GetByIdAsync(string id)
        {
            _logger.LogInformation("[REPO] GetByIdAsync called for id={Id}", id);
            var result = await _files.Find(f => f.Id == id).FirstOrDefaultAsync();
            _logger.LogInformation("[REPO] GetByIdAsync returned file={File}", result);
            return result;
        }

        public async Task CreateAsync(VehicleFile file)
        {
            _logger.LogInformation("[REPO] CreateAsync called for file={File}", file);
            await _files.InsertOneAsync(file);
            _logger.LogInformation("[REPO] CreateAsync completed");
        }
    }
}
