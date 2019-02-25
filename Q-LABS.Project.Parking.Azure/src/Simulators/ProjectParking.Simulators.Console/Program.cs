using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using ProjectParking.Simulators.Core;

namespace ProjectParking.Simulators.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var bus = Bus.Factory.CreateUsingAzureServiceBus(cfg =>
                {
                    var connectionstring = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
                    cfg.Host(connectionstring, c => { });
                });

                var simulator = new Simulator(bus, new SimulatorSettings());

               await  simulator.Start(new CancellationToken());

            }).GetAwaiter().GetResult();
            

            System.Console.ReadLine();
        }
    }
}
