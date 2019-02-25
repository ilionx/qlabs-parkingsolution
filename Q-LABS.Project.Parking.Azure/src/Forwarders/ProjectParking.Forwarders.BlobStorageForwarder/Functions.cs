using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjectParking.Contracts;
using ProjectParking.Forwarders.BlobStorageForwarder.Resources;

namespace ProjectParking.Forwarders.BlobStorageForwarder
{
    public class Functions
    {
        [NoAutomaticTrigger]
        [FunctionName("BlobStorageForwarder")]
        public static async Task ProcessMessage(TextWriter log, ILogger logger)
        {
            var forwarder = new BlobStorageForwarder(new MessagesBlobStorageRepository(logger), logger);

            var token = new CancellationToken();

            var telemetryClient = new TelemetryClient(TelemetryConfiguration.Active);

            var bus = Bus.Factory.CreateUsingAzureServiceBus(cfg =>
            {
                var connectionString = ConfigurationManager.ConnectionStrings["ServiceBusConnectionString"]
                    .ConnectionString;

                cfg.Host(connectionString, configurator => { });
                cfg.ReceiveEndpoint("BlobStorageForwarder", e => { e.Consumer(typeof(StatusUpdateConsumer), type => new StatusUpdateConsumer(forwarder, logger, telemetryClient)); });
            });

            await bus.StartAsync(token);
        }
    }

    internal class StatusUpdateConsumer : IConsumer<IParkingSpotStatusUpdate>
    {
        private readonly BlobStorageForwarder _forwarder;
        private readonly ILogger _logger;
        private TelemetryClient _telemetryClient;

        internal StatusUpdateConsumer(BlobStorageForwarder forwarder, ILogger logger, TelemetryClient telemetryClient)
        {
            _forwarder = forwarder;
            _logger = logger;
            _telemetryClient = telemetryClient;
        }

        public async Task Consume(ConsumeContext<IParkingSpotStatusUpdate> context)
        {
            _telemetryClient.TrackEvent("");
            _logger.LogInformation(JsonConvert.SerializeObject(context.Message));
            await _forwarder.Process(context.Message);

        }
    }
}
