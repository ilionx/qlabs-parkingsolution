using System.Collections.Generic;

namespace ProjectParking.WebApps.ParkingAPI.Extensions.Hateoas
{
    public class LinkInfoWrapper<T>
    {
        public T Value { get; set; }
        public List<LinkInfo> Links { get; set; }
    }
}
