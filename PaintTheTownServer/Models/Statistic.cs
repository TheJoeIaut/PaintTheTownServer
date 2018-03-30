using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaintTheTownServer.Models
{
    public class Statistic
    {
        public User User { get; set; }
        public double DistanceWalked { get; set; }
        public double AreaCovered { get; set; }
        public int AreasTaken { get; set; }
    }
}