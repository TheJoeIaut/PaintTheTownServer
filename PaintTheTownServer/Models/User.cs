using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PaintTheTownServer.Models
{
    public class User
    {[Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public Statistic Statistic { get; set; }
    }
}