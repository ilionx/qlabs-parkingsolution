using System;
using System.Configuration;

namespace ProjectParking.Processors.ParkingSpotStatusUpdateProccessor.Config
{
    public class LocationCollection : ConfigurationElementCollection
    {
        public LocationCollection()
        {
            Console.WriteLine("LocationCollection Constructor");
        }

        public Location this[int index]
        {
            get { return (Location)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(Location serviceConfig)
        {
            BaseAdd(serviceConfig);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Location();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Location) element).Name;
        }

        public void Remove(Location location)
        {
            BaseRemove(location.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }
    }
}