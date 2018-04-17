using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PaintTheTownServer.Models
{
    public class Statistic
    {
        [Key]
        public string Id { get; set; }
        //public User User { get; set; }
        public double DistanceWalked { get; set; }
        public double AreaCovered { get; set; }
        public int AreasTaken { get; set; }
    }
}