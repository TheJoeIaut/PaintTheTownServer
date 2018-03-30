using System.Data.Entity.Spatial;
using PaintTheTownServer.Models;

namespace PaintTheTownServer.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PaintTheTownServer.Models.PttContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(PaintTheTownServer.Models.PttContext context)
        {
            
        }
    }
}
