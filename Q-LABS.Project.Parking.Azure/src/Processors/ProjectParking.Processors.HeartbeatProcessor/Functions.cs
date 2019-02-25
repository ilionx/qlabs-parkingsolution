using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjectParking.Contracts;
using ProjectParking.Processors.HeartbeatProcessor.Resources;

namespace ProjectParking.Processors.HeartbeatProcessor
{
    public class Functions
    {
        private static HeartbeatProcessor _processor;

        [NoAutomaticTrigger]
        [FunctionName("HeartbeatProcessor")]
        public static async Task HeartbeatProcessor(TextWriter log, ILogger logger)
        {
            _processor = new HeartbeatProcessor(new AzureTableStorageHeartbeatRepository(logger), new ConsoleNotificationAccess(logger));

            var token = new CancellationToken();

            var bus = Bus.Factory.CreateUsingAzureServiceBus(cfg =>
            {
                var connectionString = ConfigurationManager.ConnectionStrings["ServiceBusConnectionString"]
                    .ConnectionString;

                cfg.Host(connectionString, configurator => { });
                cfg.ReceiveEndpoint("HeartbeatProcessor", e => { e.Consumer(typeof(StatusUpdateConsumer), type => new StatusUpdateConsumer(_processor, logger)); });
            });

            await bus.StartAsync(token);
            await _processor.Run(token);
        }
    }

    internal class StatusUpdateConsumer : IConsumer<IParkingSpotStatusUpdate>
    {
        private readonly HeartbeatProcessor _processor;
        private readonly ILogger _logger;

        internal StatusUpdateConsumer(HeartbeatProcessor processor, ILogger logger)
        {
            _processor = processor;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IParkingSpotStatusUpdate> context)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(context.Message));
            await _processor.Process(context.Message);

        }
    }
}