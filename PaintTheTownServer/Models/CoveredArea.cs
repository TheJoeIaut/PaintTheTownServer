using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace PaintTheTownServer.Models
{
    public class CoveredArea
    {
        public int Id { get; set; }
        public User User { get; set; }
        public DbGeography Geo { get; set; }
    }
}