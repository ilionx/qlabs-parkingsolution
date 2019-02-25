using System.Configuration;

namespace ProjectParking.Processors.ParkingSpotStatusUpdateProccessor.Config
{
    public class Location : ConfigurationElement
    {
        public Location()
        {
        }

        public Location(string name, int limit)
        {
            Name = name;
            Limit = limit;
        }

        [ConfigurationProperty("Name", DefaultValue = "Unknown", IsRequired = true, IsKey = true)]
        public string Name
        {
            get => (string) this["Name"];
            set => this["Name"] = value;
        }

        [ConfigurationProperty("Limit", DefaultValue = 0, IsRequired = true, IsKey = false)]
        public int Limit
        {
            get => (int) this["Limit"];
            set => this["Limit"] = value;
        }
    }
}