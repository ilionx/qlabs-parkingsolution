using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectParking.WebApps.ParkingAPI.Entities
{
    public class AppConfig
    {
        public IEnumerable<CarparkConfig> Carparks { get; set; }
        public string ApiKey { get;  set; }
    }
}
