using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ProjectParking.Contracts;
using ProjectParking.Contracts.enumerables;
using ProjectParking.WebApps.ParkingAPI.Entities;
using ProjectParking.WebApps.ParkingAPI.Hubs;
using ProjectParking.WebApps.ParkingAPI.Providers.Contracts;

namespace ProjectParking.WebApps.ParkingAPI.Managers
{
    public class CarparkStatusManager
    {
        private readonly ICarparkProvider carparkProvider;
        private readonly IHubContext<CarparkHub> carparkHub;
        private readonly ILogger logger;

        public CarparkStatusManager(ICarparkProvider carparkProvider, IHubContext<CarparkHub> carparkHub, ILogger<CarparkStatusManager> logger)
        {
            this.carparkProvider = carparkProvider;
            this.carparkHub = carparkHub;
            this.logger = logger;
        }

        public void ProcessUnresponsive()
        {
            DateTime now = DateTime.Now;

            IEnumerable<Carpark> carparks = this.carparkProvider.GetCarparks();

            foreach (var carpark in carparks)
            {
                List<int> unresponsiveSpots = carpark.SpotData.Where(x => (now - x.Value.LastUpdated).Seconds >= 20).Select(x => x.Key).ToList();

                foreach (var spot in unresponsiveSpots)
                {
                    ISpotUpdate update = new SpotUpdateImpl()
                    {
                        CarparkId = carpark.Id,
                        SpotId = spot,
                        Status = SpotStatus.Unknown,
                        UpdatedOn = now,
                        Source = MessageSource.System
                    };

                    ProcessUpdate(update);
                }
            }
        }

        private void ProcessUpdate(ISpotUpdate spotUpdate)
        {
            logger.LogInformation("Processing message");
            this.carparkProvider.ProcessSpotUpdate(spotUpdate);
            this.carparkHub.Clients.All.SendAsync("NotifyParkingSpotUpdate", spotUpdate);
        }


        public void ProcessStatusUpdate(IParkingSpotStatusUpdate parkingSpotStatusUpdate)
        {
            logger.LogInformation("BEGIN ProcessStatusUpdate");
            this.carparkProvider.ProcessStatusUpdate(parkingSpotStatusUpdate);
            Carpark carpark = this.carparkProvider.GetCarparkByName(parkingSpotStatusUpdate.Location);

            if (carpark == null)
            {
                logger.LogError($"No carpark found for {parkingSpotStatusUpdate.Location}");
                return;
            }

            ISpotUpdate spotUpdate = new SpotUpdateImpl()
            {
                CarparkId = carpark.Id,
                SpotId = parkingSpotStatusUpdate.SpotId,
                Status = parkingSpotStatusUpdate.Available ? SpotStatus.Available : SpotStatus.Unavailable,
                UpdatedOn = parkingSpotStatusUpdate.Timestamp,
                Source = MessageSource.MessageBus
            };

            ProcessUpdate(spotUpdate);
            
            logger.LogInformation("END ProcessStatusUpdate");
        }
    }
}
