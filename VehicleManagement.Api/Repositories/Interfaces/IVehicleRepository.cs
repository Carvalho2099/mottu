using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleManagement.Api.Models;

namespace VehicleManagement.Api.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        Task<List<Vehicle>> GetAllAsync();
        Task<Vehicle?> GetByIdAsync(string id);
        Task<List<Vehicle>> GetByPlateAsync(string plate);
        Task<List<Vehicle>> GetByIdsAsync(IEnumerable<string> ids);
        Task CreateAsync(Vehicle vehicle);
        Task UpdateAsync(string id, Vehicle vehicleIn);
        Task DeleteAsync(string id);
    }
}
