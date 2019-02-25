using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using ProjectParking.WebApps.ParkingAPI.DataTransferObjects;
using ProjectParking.WebApps.ParkingAPI.Managers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ProjectParking.WebApps.ParkingAPI.BackgroundTasks
{
    public class CarparkHubUpdateScheduledService : BackgroundService
    {
        private readonly CarparkManager carparkManager;
        private readonly ILogger logger;
        public static string Server;

        public CarparkHubUpdateScheduledService(CarparkManager carparkManager, ILogger<CarparkHubUpdateScheduledService> logger) {
            this.carparkManager = carparkManager;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested && String.IsNullOrEmpty(Server)) {

                logger.LogWarning("Server url is not yet set please wait");
                await Task.Delay(100, stoppingToken);
            }

            while (!stoppingToken.IsCancellationRequested) {

                var hubConnection = new HubConnectionBuilder()
                        .WithUrl(Server + "/broadcast")
                        .Build();
                await hubConnection.StartAsync();

                while (!stoppingToken.IsCancellationRequested)
                {
                    logger.LogInformation("BEGIN Scheduled Update-All");

                    IEnumerable<CarparkListDTO> carparks = carparkManager.GetCarparks();

                    foreach (var carpark in carparks)
                    {
                        await hubConnection.SendAsync("NotifyParkingStatusForCarpark", carpark.Id, carparkManager.GetParkingSpots(carpark.Id));
                        await Task.Delay(1000, stoppingToken);

                    }
                    logger.LogInformation("END Scheduled Update-All, Wait 5000ms for next run");

                    await Task.Delay(5000, stoppingToken);
                }
            }

        }
    }
}
