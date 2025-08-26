namespace VehicleManagement.Api.Services.Interfaces
{
    public interface IMessageQueueService
    {
        void Publish(string queue, string message);
    }
}
