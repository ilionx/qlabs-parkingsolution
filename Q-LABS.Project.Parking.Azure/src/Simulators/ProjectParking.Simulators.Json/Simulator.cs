using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Newtonsoft.Json;
using ProjectParking.Contracts;

namespace ProjectParking.Simulators.Json
{
    public class Simulator
    {
        private SpotConfiguration _config;
        private readonly IPublishEndpoint _bus;


        public Simulator(SpotConfiguration config, IPublishEndpoint bus)
        {
            _config = config;
            _bus = bus;
        }

        public void Update(SpotConfiguration config)
        {
            _config = config;
        }

        public async Task Start(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var spot in _config.Spots)
                {
                    spot.Timestamp = DateTime.Now;

                    await _bus.Publish<IParkingSpotStatusUpdate>(spot, cancellationToken);
                    Console.WriteLine($"Simulator sent: {JsonConvert.SerializeObject(spot)}.\r\n");
                }

                await Task.Delay(2000, cancellationToken);
            }
            
        }
    }
}
