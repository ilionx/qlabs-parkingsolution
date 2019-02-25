using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectParking.Contracts
{
    public interface ICarparkInfo
    {
         int AvailableSpaces { get; set; }

         int UnavailableSpaces { get; set; }

         int UnknownSpaces { get; set; }

         int TotalSpaces { get; set; }
    }
}
