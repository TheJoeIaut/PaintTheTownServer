using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PaintTheTownServer.Models
{
    public class PttContext : DbContext
    {
        public PttContext(): base("name=PttContext")
        {
           
        }

        public DbSet<User> Users { get; set; }
        public DbSet<CoveredArea>  CoveredAreas{ get; set; }
    }
}