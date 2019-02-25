using System.Collections.Generic;
using System.Configuration;

namespace ProjectParking.Processors.ParkingSpotStatusUpdateProccessor.Config
{
    public class LocationConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("Locations", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(LocationCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public LocationCollection Locations
        {
            get
            {
                return (LocationCollection)base["Locations"];
            }
        }
    }
}