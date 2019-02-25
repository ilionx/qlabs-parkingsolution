using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjectParking.Contracts;
using ProjectParking.Processors.ParkingSpotStatusUpdateProccessor.Config;

namespace ProjectParking.Processors.ParkingSpotStatusUpdateProccessor
{
    public class Functions
    {
        private static List<Location> _locations;
        private static StatusUpdateConsumer _processor;

        [NoAutomaticTrigger]
        [FunctionName("ParkingSpotStatusUpdateProcessor")]
        public static async Task ParkingSpotStatusUpdateProcessor(TextWriter log, ILogger logger)
        {
            var locationConfigSection = GetLocationsConfiguration();

            _locations = (from Location location in locationConfigSection.Locations
                select location).ToList();

            _processor = new StatusUpdateConsumer(logger, _locations);

            var bus = Bus.Factory.CreateUsingAzureServiceBus(cfg =>
            {
                var connectionString = ConfigurationManager.ConnectionStrings["ServiceBusConnectionString"]
                    .ConnectionString;
                cfg.ConfigureJsonSerializer(settings => new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.None });
                cfg.Host(connectionString, configurator => { });
                cfg.ReceiveEndpoint("ParkingSpotStatusUpdateProcessor",
                    e =>
                    {
                        e.Consumer(typeof(StatusUpdateConsumer), type => _processor);
                    });
            });

            await bus.StartAsync();
        }

        private static LocationConfigurationSection GetLocationsConfiguration()
        {
            LocationConfigurationSection locationConfigSection =
                ConfigurationManager.GetSection("LocationCollection") as LocationConfigurationSection;

            Debug.Assert(locationConfigSection != null, nameof(locationConfigSection) + " != null");
            return locationConfigSection;
        }
    }

    public class StatusUpdateConsumer : IConsumer<IParkingSpotStatusUpdate>
    {
        private readonly ILogger _logger;
        private readonly List<Location> _locations;


        public StatusUpdateConsumer(ILogger logger, List<Location> locations)
        {
            _logger = logger;
            _locations = locations;

            _logger.LogInformation($"Started with location settings: {JsonConvert.SerializeObject(locations, Formatting.Indented)}");
        }

        public Task Consume(ConsumeContext<IParkingSpotStatusUpdate> context)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(context.Message));
            return Task.CompletedTask;
        }
    }
}