using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ParkingAppQLABS.Models
{
    public class CarparkListDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public LocationDTO location { get; set; }
        public int availableParkingSpots { get; set; }
        public Color color { get; set; }
    }
}
