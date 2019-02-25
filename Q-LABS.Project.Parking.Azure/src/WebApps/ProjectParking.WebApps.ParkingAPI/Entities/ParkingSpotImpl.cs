using ProjectParking.Contracts.enumerables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectParking.WebApps.ParkingAPI.Entities
{
    public class ParkingSpotImpl
    {
        public int SpotId { get; set; }
        public SpotStatus SpotStatus { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
