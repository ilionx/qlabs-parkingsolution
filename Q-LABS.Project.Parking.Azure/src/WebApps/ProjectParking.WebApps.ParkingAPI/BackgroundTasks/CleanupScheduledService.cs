using ProjectParking.WebApps.ParkingAPI.Providers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ProjectParking.WebApps.ParkingAPI.Managers;

namespace ProjectParking.WebApps.ParkingAPI.BackgroundTasks
{
    public class CleanupScheduledService : BackgroundService
    {
        private readonly CarparkStatusManager carparkStatusManager;
        private readonly ILogger logger;

        public CleanupScheduledService(CarparkStatusManager carparkStatusManager, ILogger<CleanupScheduledService> logger) {
            this.carparkStatusManager = carparkStatusManager;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("BEGIN Scheduled Clean-Up");
                carparkStatusManager.ProcessUnresponsive();
                logger.LogInformation("END Scheduled Clean-Up, Wait 5000ms for next run");
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
