using ProjectParking.Contracts;
using ProjectParking.Contracts.enumerables;
using System;

namespace ProjectParking.WebApps.ParkingAPI.DataTransferObjects
{
    public class LocationDTO: ILocation
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
