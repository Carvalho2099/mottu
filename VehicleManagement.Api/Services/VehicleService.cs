using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleManagement.Api.Models;
using VehicleManagement.Api.Repositories.Interfaces;
using VehicleManagement.Api.Services.Interfaces;

namespace VehicleManagement.Api.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMessageQueueService _messageQueueService;
        private readonly ILogger<VehicleService> _logger;

        public VehicleService(IVehicleRepository vehicleRepository, IMessageQueueService messageQueueService, ILogger<VehicleService> logger)
        {
            _vehicleRepository = vehicleRepository;
            _messageQueueService = messageQueueService;
            _logger = logger;
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesAsync(string? plate, string? ids)
        {
            _logger.LogInformation("[SERVICE] GetVehiclesAsync called with plate={Plate}, ids={Ids}", plate, ids);
            if (!string.IsNullOrEmpty(plate))
            {
                var result = await _vehicleRepository.GetByPlateAsync(plate);
                _logger.LogInformation("[SERVICE] GetVehiclesAsync by plate found {Count} vehicles", result.Count);
                return result;
            }
            else if (!string.IsNullOrEmpty(ids))
            {
                var idList = ids.Split(',');
                var result = await _vehicleRepository.GetByIdsAsync(idList);
                _logger.LogInformation("[SERVICE] GetVehiclesAsync by ids found {Count} vehicles", result.Count);
                return result;
            }
            else
            {
                var result = await _vehicleRepository.GetAllAsync();
                _logger.LogInformation("[SERVICE] GetVehiclesAsync all found {Count} vehicles", result.Count);
                return result;
            }
        }

        public async Task CreateVehicleAsync(Vehicle vehicle)
        {
            _logger.LogInformation("[SERVICE] CreateVehicleAsync called for plate={Plate}", vehicle.Plate);
            await _vehicleRepository.CreateAsync(vehicle);
            var message = System.Text.Json.JsonSerializer.Serialize(new { Event = "VehicleCreated", Data = vehicle });
            _messageQueueService.Publish("vehicle_events", message);
            _logger.LogInformation("[SERVICE] CreateVehicleAsync published event for plate={Plate}", vehicle.Plate);
        }

        public async Task<Vehicle?> GetVehicleByIdAsync(string id)
        {
            return await _vehicleRepository.GetByIdAsync(id);
        }

        public async Task UpdateVehicleAsync(string id, Vehicle vehicle)
        {
            _logger.LogInformation("[SERVICE] UpdateVehicleAsync called for id={Id}", id);
            await _vehicleRepository.UpdateAsync(id, vehicle);
            _logger.LogInformation("[SERVICE] UpdateVehicleAsync updated vehicle id={Id}", id);
        }

        public async Task DeleteVehicleAsync(string id)
        {
            _logger.LogInformation("[SERVICE] DeleteVehicleAsync called for id={Id}", id);
            await _vehicleRepository.DeleteAsync(id);
            _logger.LogInformation("[SERVICE] DeleteVehicleAsync deleted vehicle id={Id}", id);
        }
    }
}
