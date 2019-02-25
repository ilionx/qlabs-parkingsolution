using ProjectParking.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectParking.WebApps.ParkingAPI.DataTransferObjects
{
    public class CarparkInfoDTO: ICarparkInfo
    {
        public int AvailableSpaces { get; set; }

        public int UnavailableSpaces { get; set; }

        public int UnknownSpaces { get; set; }

        public int TotalSpaces { get; set; }
    }
}
