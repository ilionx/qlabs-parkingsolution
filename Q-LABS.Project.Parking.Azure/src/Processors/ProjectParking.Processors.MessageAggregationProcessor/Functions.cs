using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProjectParking.Contracts;
using ProjectParking.Processors.MessageAggregationProcessor.Resources;

namespace ProjectParking.Processors.MessageAggregationProcessor
{
    public class Functions
    {
        [NoAutomaticTrigger]
        [FunctionName("MessageAggregationProcessor")]
        public static async Task MessageAggregationProcessor(TextWriter log, ILogger logger)
        {
            var bus = Bus.Factory.CreateUsingAzureServiceBus(cfg =>
            {
                var connectionString = ConfigurationManager.ConnectionStrings["ServiceBusConnectionString"]
                    .ConnectionString;

                cfg.Host(connectionString, configurator => { });

                cfg.ReceiveEndpoint("MessageAggregationProcessor",
                    e => { e.Consumer(typeof(StatusUpdateConsumer), type => new StatusUpdateConsumer(logger)); });
            });

            await bus.StartAsync();
        }
    }

    public class StatusUpdateConsumer : IConsumer<IParkingSpotStatusUpdate>
    {
        private readonly ILogger _logger;
        private readonly IParkingSpotMessageProvider _parkingspotMessageProvider;

        public StatusUpdateConsumer(ILogger logger)
        {
            _logger = logger;
            _parkingspotMessageProvider = new ParkingSpotMessageTableStorageProvider(logger);
        }

        public async Task Consume(ConsumeContext<IParkingSpotStatusUpdate> context)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(context.Message));
            await _parkingspotMessageProvider.Store(context.Message);
        }
    }
}