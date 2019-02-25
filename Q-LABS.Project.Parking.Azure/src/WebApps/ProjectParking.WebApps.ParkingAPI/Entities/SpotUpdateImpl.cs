using System;

using ProjectParking.Contracts;
using ProjectParking.Contracts.enumerables;

namespace ProjectParking.WebApps.ParkingAPI.Entities
{
    public class SpotUpdateImpl: ISpotUpdate
    {
        public int CarparkId { get; set; }
        public int SpotId { get; set; }

        public DateTime UpdatedOn { get; set; }

        public SpotStatus Status { get; set; }

        public MessageSource Source { get; set; }
    }
}
