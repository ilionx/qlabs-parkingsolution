using ProjectParking.WebApps.ParkingAPI.Entities;
using System.Collections.ObjectModel;

namespace ProjectParking.WebApps.ParkingAPI.Providers.Contracts.Entities
{
    public class Carparks : KeyedCollection<int, Carpark>
    {
        protected override int GetKeyForItem(Carpark item)
        {
            return item.Id;
        }
    }
}
