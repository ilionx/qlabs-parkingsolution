using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace ProjectParking.Processors.HeartbeatProcessor.Entities
{
    public class ParkingSpotStatusUpdateEntity : TableEntity
    {
        public ParkingSpotStatusUpdateEntity()
        {
        }

        public ParkingSpotStatusUpdateEntity(int spotId, string location, DateTime timestamp)
        {
            PartitionKey = location;
            RowKey = spotId.ToString();

            SpotId = spotId;
            Location = location;
            UpdateTimestamp = timestamp.ToUniversalTime();
            FailedToReply = false;
        }

        public int SpotId { get; set; }
        public string Location { get; set; }
        public DateTime UpdateTimestamp { get; set; }
        public bool FailedToReply { get; set; }
    }
}