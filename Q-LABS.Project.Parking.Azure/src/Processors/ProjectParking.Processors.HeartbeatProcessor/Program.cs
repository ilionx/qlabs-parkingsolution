using System;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ProjectParking.Processors.HeartbeatProcessor
{
    internal class Program
    {
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

                host.CallAsync(typeof(Functions).GetMethod(nameof(Functions.HeartbeatProcessor)));
                host.RunAndBlock();
            }
        }
    }
}