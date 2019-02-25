using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectParking.WebApps.ParkingAPI.Entities
{
    public class CarparkConfig
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalSpots { get; set; }
        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
