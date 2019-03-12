using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingAppQLABS.Models
{
    public class LinkInfoWrapper_CarparkListDTO
    {
        public CarparkListDTO value { get; set; }
        public List<LinkInfo> links { get; set; }
    }
}
