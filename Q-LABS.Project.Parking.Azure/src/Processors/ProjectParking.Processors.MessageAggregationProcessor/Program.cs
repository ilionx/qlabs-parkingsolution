using System.Configuration;
using System.Diagnostics;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ProjectParking.Processors.MessageAggregationProcessor
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    internal class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        private static void Main()
        {
            using (var loggerFactory = new LoggerFactory())
            {
                var config = new JobHostConfiguration();
                config.LoggerFactory = loggerFactory
                    .AddConsole();
                config.Tracing.ConsoleLevel = TraceLevel.Off;

                string instrumentationKey = ConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"];
                if (!string.IsNullOrEmpty(instrumentationKey))
                {
                    config.LoggerFactory.AddApplicationInsights(instrumentationKey, null);
                }

                if (config.IsDevelopment) config.UseDevelopmentSettings();

                var host = new JobHost(config);

                host.CallAsync(typeof(Functions).GetMethod(nameof(Functions.MessageAggregationProcessor)));

                host.RunAndBlock();
            }
        }
    }
}