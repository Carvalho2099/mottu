using MongoDB.Driver;
using VehicleManagement.Api.Models;
using Microsoft.Extensions.Configuration;

namespace VehicleManagement.Api.Repositories
{
    using VehicleManagement.Api.Repositories.Interfaces;

    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(IConfiguration configuration, ILogger<UserRepository> logger)
        {
            var connStr = configuration["Mongo:ConnectionString"];
            logger.LogInformation("Mongo:ConnectionString: {ConnStr}", connStr);
            var client = new MongoClient(connStr);
            var database = client.GetDatabase("vehicle_management");
            _users = database.GetCollection<User>("Users");
            _logger = logger;
        }

        // Construtor para testes
        public UserRepository(IMongoCollection<User> usersCollection, ILogger<UserRepository> logger)
        {
            _users = usersCollection;
            _logger = logger;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            _logger.LogInformation("Getting user by username: {Username}", username);
            return await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task<User?> GetByUsernameAndPasswordAsync(string username, string password)
        {
            _logger.LogInformation("Getting user by username and password: {Username}", username);
            return await _users.Find(u => u.Username == username && u.Password == password).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetAllAsync() => await _users.Find(_ => true).ToListAsync();

        public async Task CreateAsync(User user) => await _users.InsertOneAsync(user);

        public async Task UpdateAsync(string id, User userIn) =>
            await _users.ReplaceOneAsync(u => u.Id == id, userIn);

        public async Task DeleteAsync(string id) =>
            await _users.DeleteOneAsync(u => u.Id == id);
    }
}
