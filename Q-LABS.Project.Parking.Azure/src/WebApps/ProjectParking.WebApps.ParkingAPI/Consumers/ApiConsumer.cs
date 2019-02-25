using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using ProjectParking.Contracts;
using ProjectParking.WebApps.ParkingAPI.Managers;

namespace ProjectParking.WebApps.ParkingAPI.Consumers
{
    internal class ApiConsumer : IConsumer<IParkingSpotStatusUpdate>
    {
        private readonly CarparkStatusManager carparkStatusManager;
        private readonly ILogger logger;

        public ApiConsumer(CarparkStatusManager carparkStatusManager, ILogger<ApiConsumer> logger)
        {
            this.carparkStatusManager = carparkStatusManager;
            this.logger = logger;
        }

        public Task Consume(ConsumeContext<IParkingSpotStatusUpdate> context)
        {
            logger.LogInformation($"Status Context received {context.MessageId}");

            carparkStatusManager.ProcessStatusUpdate(context.Message);
            return Task.CompletedTask;
        }
    }
}
