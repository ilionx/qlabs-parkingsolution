using ProjectParking.Contracts;
using ProjectParking.Contracts.enumerables;
using System;

namespace ProjectParking.WebApps.ParkingAPI.DataTransferObjects
{
    public class ParkingSpotDTO: IParkingSpot
    {
        public int CarparkId { get; set; }
        public int SpotId { get; set; }
        public DateTime LastUpdated { get; set; }
        public SpotStatus Status { get; set; }
    }
}
