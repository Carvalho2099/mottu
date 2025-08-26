using MongoDB.Driver;
using VehicleManagement.Api.Models;
using Microsoft.Extensions.Configuration;

namespace VehicleManagement.Api.Repositories
{
    using VehicleManagement.Api.Repositories.Interfaces;

public class VehicleRepository : IVehicleRepository
    {
        private readonly IMongoCollection<Vehicle> _vehicles;
        private readonly ILogger<VehicleRepository> _logger;

        // Construtor para produção
        public VehicleRepository(IConfiguration configuration, ILogger<VehicleRepository> logger)
        {
            var connStr = configuration["Mongo:ConnectionString"];
            var client = new MongoClient(connStr);
            var database = client.GetDatabase("vehicle_management");
            _vehicles = database.GetCollection<Vehicle>("Vehicles");
            _logger = logger;
        }

        // Construtor para testes
        public VehicleRepository(IMongoCollection<Vehicle> vehiclesCollection, ILogger<VehicleRepository> logger)
        {
            _vehicles = vehiclesCollection;
            _logger = logger;
        }

        public async Task<List<Vehicle>> GetAllAsync()
        {
            using (_logger.BeginScope("GetAllAsync"))
            {
                _logger.LogInformation("GetAllAsync called");
                var result = await _vehicles.Find(_ => true).ToListAsync();
                _logger.LogInformation("GetAllAsync returned {Count} vehicles", result.Count);
                return result;
            }
        }

        public async Task<Vehicle?> GetByIdAsync(string id)
        {
            _logger.LogInformation("[REPO] GetByIdAsync called for id={Id}", id);
            var result = await _vehicles.Find(v => v.Id == id).FirstOrDefaultAsync();
            _logger.LogInformation("[REPO] GetByIdAsync result is {Result}", result != null ? "found" : "not found");
            return result;
        }

        public async Task<List<Vehicle>> GetByPlateAsync(string plate)
        {
            _logger.LogInformation("[REPO] GetByPlateAsync called for plate={Plate}", plate);
            var result = await _vehicles.Find(v => v.Plate == plate).ToListAsync();
            _logger.LogInformation("[REPO] GetByPlateAsync returned {Count} vehicles", result.Count);
            return result;
        }

        public async Task<List<Vehicle>> GetByIdsAsync(IEnumerable<string> ids)
        {
            _logger.LogInformation("[REPO] GetByIdsAsync called for ids={Ids}", string.Join(",", ids));
            var result = await _vehicles.Find(v => ids.Contains(v.Id)).ToListAsync();
            _logger.LogInformation("[REPO] GetByIdsAsync returned {Count} vehicles", result.Count);
            return result;
        }

        public async Task CreateAsync(Vehicle vehicle)
        {
            _logger.LogInformation("[REPO] CreateAsync called for plate={Plate}", vehicle.Plate);
            await _vehicles.InsertOneAsync(vehicle);
            _logger.LogInformation("[REPO] CreateAsync completed for plate={Plate}", vehicle.Plate);
        }

        public async Task UpdateAsync(string id, Vehicle vehicleIn)
        {
            _logger.LogInformation("[REPO] UpdateAsync called for id={Id}", id);
            await _vehicles.ReplaceOneAsync(v => v.Id == id, vehicleIn);
            _logger.LogInformation("[REPO] UpdateAsync completed for id={Id}", id);
        }

        public async Task DeleteAsync(string id)
        {
            _logger.LogInformation("[REPO] DeleteAsync called for id={Id}", id);
            await _vehicles.DeleteOneAsync(v => v.Id == id);
            _logger.LogInformation("[REPO] DeleteAsync completed for id={Id}", id);
        }
    }
}
