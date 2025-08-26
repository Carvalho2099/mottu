using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleManagement.Api.Models;

namespace VehicleManagement.Api.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle>> GetVehiclesAsync(string? plate, string? ids);
        Task CreateVehicleAsync(Vehicle vehicle);
        Task<Vehicle?> GetVehicleByIdAsync(string id);
        Task UpdateVehicleAsync(string id, Vehicle vehicle);
        Task DeleteVehicleAsync(string id);
    }
}
