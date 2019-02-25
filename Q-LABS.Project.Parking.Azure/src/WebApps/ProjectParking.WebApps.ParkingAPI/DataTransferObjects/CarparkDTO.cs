using ProjectParking.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectParking.WebApps.ParkingAPI.DataTransferObjects
{
    public class CarparkDTO: ICarpark
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ICarparkInfo Info { get; set; }

        public ILocation Location { get; set; }

    }
}
