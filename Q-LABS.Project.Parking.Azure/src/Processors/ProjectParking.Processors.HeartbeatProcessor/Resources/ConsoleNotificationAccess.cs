using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ProjectParking.Processors.HeartbeatProcessor.Resources
{
    internal class ConsoleNotificationAccess : INotificationAccess
    {
        private readonly ILogger _logger;

        public ConsoleNotificationAccess(ILogger logger)
        {
            _logger = logger;
        }

        public Task SendNotificationFor(params UnresponsiveParkingSpot[] spots)
        {
            using (var scope = _logger.BeginScope(spots))
            {
                var sb = new StringBuilder();
                sb.AppendLine($"\r\nAdmin notification: The following spots are unresponsive;");
                sb.AppendLine();
                foreach (var locationSpots in spots.GroupBy(x => x.Location))
                {
                    sb.AppendLine($"Location: {locationSpots.First().Location} - Spots: {string.Join(", ", locationSpots.Select(x => x.SpotId))}");
                }

                sb.AppendLine();
                _logger.LogWarning(sb.ToString());
                return Task.CompletedTask;
            }
        }
    }
}