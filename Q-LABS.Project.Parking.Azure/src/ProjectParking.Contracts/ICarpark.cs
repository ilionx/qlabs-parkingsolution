using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectParking.Contracts
{
    public interface ICarpark
    {
         string Name { get; set; }

         string Description { get; set; }

         ICarparkInfo Info { get; set; }

         ILocation Location { get; set; }
    }
}
