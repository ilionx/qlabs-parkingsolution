using ProjectParking.Contracts;
using ProjectParking.WebApps.ParkingAPI.Entities;
using System.Collections.Generic;

namespace ProjectParking.WebApps.ParkingAPI.Providers.Contracts
{
    public interface ICarparkProvider
    {
        IEnumerable<Carpark> GetCarparks();

        Carpark GetCarpark(int id);

        Carpark GetCarparkByName(string name);

        void AddCarpark(Carpark carpark);

        void ProcessStatusUpdate(IParkingSpotStatusUpdate parkingSpotStatusUpdate);

        void ProcessSpotUpdate(ISpotUpdate spotUpdate);

        void CleanUp();
    }
}
