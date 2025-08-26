using System.Threading.Tasks;
using VehicleManagement.Api.Models;

namespace VehicleManagement.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User?> AuthenticateAsync(string username, string password);
        string GenerateJwtToken(User user);
    }
}
