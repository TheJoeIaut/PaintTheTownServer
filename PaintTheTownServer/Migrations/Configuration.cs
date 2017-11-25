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
            var testuser = new User()
            {
                Name = "Testuser"
            };

            context.Users.AddOrUpdate(x=>x.Name,testuser );

            if (!context.CoveredAreas.Any())
            {
                context.CoveredAreas.AddRange(new[]
                {
                    new CoveredArea
                    {
                        User = testuser,
                        Geo = DbGeography.PolygonFromText(
                            "POLYGON((16.321349143981934 48.21034849634383,16.331048011779785 48.21034849634383,16.331048011779785 48.20497166007089,16.321349143981934 48.20497166007089,16.321349143981934 48.21034849634383))",
                            4326)
                    },
                    new CoveredArea
                    {
                        User = testuser,
                        Geo = DbGeography.PolygonFromText(
                            "POLYGON((16.324138641357422 48.203255529666116,16.32619857788086 48.20725974450292,16.339759826660156 48.20971932126293,16.340274810791016 48.20268347342048,16.324138641357422 48.203255529666116))",
                            4326)
                    }
                });
            }

            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
