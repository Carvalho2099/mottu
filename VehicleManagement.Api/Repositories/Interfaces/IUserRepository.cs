using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleManagement.Api.Models;

namespace VehicleManagement.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByUsernameAndPasswordAsync(string username, string password);
        Task<List<User>> GetAllAsync();
        Task CreateAsync(User user);
        Task UpdateAsync(string id, User userIn);
        Task DeleteAsync(string id);
    }
}
