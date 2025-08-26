using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleManagement.Api.Models;

namespace VehicleManagement.Api.Repositories.Interfaces
{
    public interface IVehicleFileRepository
    {
        Task<List<VehicleFile>> GetByVehicleIdAsync(string vehicleId);
        Task<VehicleFile?> GetByIdAsync(string id);
        Task CreateAsync(VehicleFile file);
    }
}
