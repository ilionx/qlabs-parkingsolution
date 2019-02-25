using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Bogus.Extensions;
using MassTransit;
using Newtonsoft.Json;
using ProjectParking.Contracts;


namespace ProjectParking.Simulators.Core
{
    public class SimulatorSettings
    {
        public int IntervalBetweenBurstsInMs = int.Parse(ConfigurationManager.AppSettings[nameof(IntervalBetweenBurstsInMs)]);
        public int MaxMessagePerBurst = int.Parse(ConfigurationManager.AppSettings[nameof(MaxMessagePerBurst)]);
        public int MinMessagePerBurst = int.Parse(ConfigurationManager.AppSettings[nameof(MinMessagePerBurst)]);
    }

    public class Simulator
    {
        private readonly IPublishEndpoint _bus;
        private readonly SimulatorSettings _settings;


        public Simulator(IPublishEndpoint bus, SimulatorSettings settings)
        {
            _bus = bus;
            _settings = settings;
        }

        public async Task Start(CancellationToken cancellationToken)
        {
            Console.WriteLine("Started simulator...");
            var numberFaker = new Faker();

            while (!cancellationToken.IsCancellationRequested)
            {
                var messages = ParkingSpotStatusUpdate.Random(_settings.MinMessagePerBurst, _settings.MaxMessagePerBurst);

                messages.ForEach(async message =>
                {
                    await _bus.Publish<IParkingSpotStatusUpdate>(message, cancellationToken);
                    Console.WriteLine($"Simulator sent: {JsonConvert.SerializeObject(message)}.\r\n");
                });

                await Task.Delay(_settings.IntervalBetweenBurstsInMs, cancellationToken);
            }

            Console.WriteLine("Stopped simulator.");
        }
    }

    public class ParkingSpotStatusUpdate : IParkingSpotStatusUpdate
    {
        private static Faker<ParkingSpotStatusUpdate> _parkingSpotFakes = new Faker<ParkingSpotStatusUpdate>()
            .StrictMode(true)
            .RuleFor(o => o.SpotId, f => f.Random.Number(1, 48))
            .RuleFor(o => o.Available, f => f.Random.Bool())
            .RuleFor(o => o.Location, f => f.PickRandom(_locations))
            .RuleFor(o => o.Timestamp, f => DateTime.Now)
            .RuleFor(o => o.MetaData, f => new Dictionary<string, string>()
            {
                { "carColor", $"{f.Internet.Color()}" },
                { "longitude", $"{f.Address.Longitude()}" },
                { "latitude", $"{f.Address.Latitude()}" }
            });

        private static string[] _locations = new[] { "QNH Consulting Zuid B.V.", "ENGIE Services Zuid B.V.", "Race Art", "Carconnect" };

        public static List<ParkingSpotStatusUpdate> Random(int min, int max)
        {
            return _parkingSpotFakes.GenerateBetween(min, max);
        }

        public int SpotId { get; set; }
        public string Location { get; set; }
        public bool Available { get; set; }
        public DateTime Timestamp { get; set; }
        public IDictionary<string, string> MetaData { get; set; }
    }
}
