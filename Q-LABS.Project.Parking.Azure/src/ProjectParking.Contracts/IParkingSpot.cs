using ProjectParking.Contracts.enumerables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectParking.Contracts
{
    public interface IParkingSpot
    {
        int CarparkId { get; set; }

        int SpotId { get; set; }

        DateTime LastUpdated { get; set; }

        SpotStatus Status { get; set; }
    }
}
