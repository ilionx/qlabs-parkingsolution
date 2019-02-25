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

namespace ProjectParking.Simulators.Json
{
    class Program
    {
        private static Simulator _simulator;

        static void Main(string[] args)
        {
            string fileName = args.FirstOrDefault();
            if (string.IsNullOrEmpty(fileName))
                fileName = "parking.json";

            var configPath = Path.Combine(Environment.CurrentDirectory, fileName);

            

            Console.WriteLine($"Using {configPath}");

            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(configPath), "*.json");
            fileSystemWatcher.EnableRaisingEvents = true;
            fileSystemWatcher.Changed += FileSystemWatcher_Changed;


            SpotConfiguration config = SpotConfiguration.Parse(configPath);

            Task.Run(async () =>
            {
                var bus = Bus.Factory.CreateUsingAzureServiceBus(cfg =>
                {
                    var connectionstring = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
                    cfg.Host(connectionstring, c => { });
                });

                _simulator = new Simulator(config, bus);

                await _simulator.Start(CancellationToken.None);

            }).GetAwaiter().GetResult();


            System.Console.ReadLine();
        }

        private static void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            var config = SpotConfiguration.Parse(e.FullPath);
            _simulator.Update(config);
        }
    }
}
