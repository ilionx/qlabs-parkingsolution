using System.Collections.Concurrent;

namespace ProjectParking.WebApps.ParkingAPI.Entities
{
    public class Carpark
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int TotalSpaces { get; set; }

        public ConcurrentDictionary<int, ParkingSpotImpl> SpotData { get; set; } = new ConcurrentDictionary<int, ParkingSpotImpl>();
    }
}
