using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectParking.Contracts.enumerables;

namespace ProjectParking.Contracts
{
    public interface ISpotUpdate
    {
        int CarparkId { get; set; }
        int SpotId { get; set; }

        DateTime UpdatedOn { get; set; }

        SpotStatus Status { get; set; }

        MessageSource Source { get; set; }
    }
}
