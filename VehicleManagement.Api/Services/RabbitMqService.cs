using System.Text;
using RabbitMQ.Client;
using VehicleManagement.Api.Services.Interfaces;

namespace VehicleManagement.Api.Services
{
    public class RabbitMqService : IMessageQueueService
    {
        private readonly string _hostname;
        private readonly string _username;
        private readonly string _password;
        private readonly IConnection _connection;
        private readonly ILogger<RabbitMqService> _logger;

        public RabbitMqService(IConfiguration configuration, ILogger<RabbitMqService> logger)
        {
            _hostname = configuration["RabbitMQ__Host"] ?? "localhost";
            _username = configuration["RabbitMQ__User"] ?? "guest";
            _password = configuration["RabbitMQ__Password"] ?? "guest";

            var factory = new ConnectionFactory()
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };
            _connection = factory.CreateConnection();
            _logger = logger;
        }

        public void Publish(string queue, string message)
        {
            try
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "", routingKey: queue, basicProperties: null, body: body);
                    _logger.LogInformation("[RABBITMQ] Message published to queue {Queue}: {Message}", queue, message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RABBITMQ] Failed to publish message to queue {Queue}: {Message}", queue, message);
                throw;
            }
        }
    }
}
