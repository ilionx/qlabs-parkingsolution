using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using ProjectParking.Contracts;

namespace ProjectParking.Functions.SensorFeed
{
    public static class SensorFeedApi
    {
        [FunctionName("parkingspots")]
        public static void Run(
             [HttpTrigger(AuthorizationLevel.Function, "post", Route = "parkingspots/status")]
            ParkingSpotStatusUpdate[] inputs, [ServiceBus("projectparking.contracts/iparkingspotstatusupdate", Connection = "ServiceBusConnectionString")]ICollector<BrokeredMessage> output, TraceWriter log, ExecutionContext context)
        {

            foreach (var input in inputs)
            {
                var sw = Stopwatch.StartNew();
                var destinationAddress = GetDestinationAddress();

                var json = JsonConvert.SerializeObject(new MassTransitEnvelope
                {
                    message = input,
                    destinationAddress = destinationAddress.ToString(),
                    headers = new Headers(),
                    messageType = new[]
                    {
                        "urn:message:ProjectParking.Contracts:IParkingSpotStatusUpdate"
                    }
                });

                var message = GenerateBrokeredMessage(json);

                sw.Stop();

                output.Add(message);
            }
        }

        private static Uri GetDestinationAddress()
        {
            var x = new ServiceBusConnectionStringBuilder(ConfigurationManager.ConnectionStrings["ServiceBusConnectionString"].ConnectionString);

            var parts = new string[3]
            {
                x.Endpoints.First().ToString(),
                typeof(IParkingSpotStatusUpdate).Namespace,
                typeof(IParkingSpotStatusUpdate).Name
            };


            var path = $"{typeof(IParkingSpotStatusUpdate).Namespace}/{typeof(IParkingSpotStatusUpdate).Name}";
            var destinationAddress = new Uri(x.Endpoints.First(), path);
            return destinationAddress;
        }

        private static BrokeredMessage GenerateBrokeredMessage(string json)
        {
            // Required for MassTransit compatibility
            var bytes = Encoding.UTF8.GetBytes(json);
            var stream = new MemoryStream(bytes, false);

            var message = new BrokeredMessage(stream)
            {
                ContentType = "application/vnd.masstransit+json"
            };

            return message;
        }
    }
}