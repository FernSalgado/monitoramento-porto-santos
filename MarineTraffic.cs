using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIClimaTempo
{
    public class MarineTraffic
    {
        public int MMSI { get; set; }
        public double Speed { get; set; }
        public DateTime Data { get; set; }
    }
}
