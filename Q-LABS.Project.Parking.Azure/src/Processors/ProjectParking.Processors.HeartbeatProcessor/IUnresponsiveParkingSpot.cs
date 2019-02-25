using System;

namespace ProjectParking.Processors.HeartbeatProcessor
{
    public interface IUnresponsiveParkingSpot
    {
        int SpotId { get; }
        string Location { get; }
        
        DateTime LastReceievedTimestamp { get; }
    }

    public class UnresponsiveParkingSpot : IUnresponsiveParkingSpot
    {
        public UnresponsiveParkingSpot(int spotId, string location, DateTime lastReceievedTimestamp)
        {
            SpotId = spotId;
            Location = location;
            LastReceievedTimestamp = lastReceievedTimestamp;
        }

        public int SpotId { get; }
        public string Location { get; }
        public DateTime LastReceievedTimestamp { get; }
    }
}