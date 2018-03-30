using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Data.Entity.SqlServer;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using GeoJSON.Net.Geometry;
using GeoJSON.Net.Contrib.EntityFramework;
using GeoJSON.Net.Feature;
using Newtonsoft.Json;
using PaintTheTownServer.Models;
using Microsoft.ApplicationInsights;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json.Linq;
using PaintTheTownServer.Filter;

namespace PaintTheTownServer.Controllers
{
    [JWTAuthenticationFilter]
    public class MapController : ApiController
    {
        [HttpPost]
        [ActionName("GetPolysInArea")]
        public async Task<FeatureCollection> GetPolysInArea()
        {
           
                string json = await Request.Content.ReadAsStringAsync();
            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient();
            telemetry.TrackEvent("STH", new Dictionary<string, string> {{"body", json.Replace(@"\", "").TrimStart('"').TrimEnd('"') } });
            telemetry.Flush();
         
            var currentPostion = JsonConvert.DeserializeObject<GeoJSON.Net.Geometry.Point>(json.Replace(@"\", "").TrimStart('"').TrimEnd('"'));

                var currentPositionDbGeography = currentPostion.ToDbGeography();

                var result = new FeatureCollection();

                using (var context = new PttContext())
                {
                    foreach (var nearArea in context.CoveredAreas.Where(
                        x => x.Geo.Distance(currentPositionDbGeography) < 10000))
                    {
                        result.Features.Add(new Feature(nearArea.Geo.ToGeoJSONGeometry()));
                    }

                    var firstGeo = context.CoveredAreas.First(x => x.Id == 3).Geo;


                    var y =context.CoveredAreas.Where(x => x.Geo.Intersects(firstGeo)).ToList();

                    foreach (var intersectingArea in y)
                    {
                        intersectingArea.Geo = intersectingArea.Geo.Difference(firstGeo);
                    }


                telemetry.TrackEvent("ResultCount", new Dictionary<string, string> { { "xxxxx", result.Features.Count.ToString()} });
                return result;
                }
           
        }

        [HttpPost]
        [ActionName("SaveArea")]
        public async void SaveArea()
        {
            var json = await Request.Content.ReadAsStringAsync();
            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient();
            telemetry.TrackEvent("STH", new Dictionary<string, string> { { "body", json.Replace(@"\", "").TrimStart('"').TrimEnd('"') } });
            telemetry.Flush();
            var dbGeo = JsonConvert.DeserializeObject<Polygon>(json.Replace(@"\", "").TrimStart('"').TrimEnd('"')).ToDbGeography();
            var sqlGeography = MakePolygonValid(dbGeo);
            sqlGeography = sqlGeography.Reduce(1);
            var newPolygon = DbGeography.FromBinary(sqlGeography.STAsBinary().Value);
            using (var context = new PttContext())
            {
                var intersectingAreas = context.CoveredAreas.Where(x => x.Geo.Intersects(newPolygon)).ToList();
                

                var user = GetUser(context);

                if (newPolygon.Length != null)
                    user.Statistic.DistanceWalked += (double)newPolygon.Length;


                foreach (var intersectingArea in intersectingAreas)
                {
                    if(intersectingArea.User==user)
                        intersectingArea.Geo = intersectingArea.Geo.Difference(newPolygon);
                }

               

                context.CoveredAreas.Add(new CoveredArea()
                {
                    User = user,
                    Geo = newPolygon
                });

                context.SaveChanges();
            }
        }

        private static User GetUser(PttContext context)
        {
            var subject = GetUserSubject();

            if (!context.Users.Any(x => x.Id == subject))
            {
                context.Users.Add(new User
                {
                    Id = subject,
                    Name = ""
                });
            }

            var user = context.Users.FirstOrDefault(x => x.Id == subject);
            return user;
        }

        private static string GetUserSubject()
        {
            var principal = Thread.CurrentPrincipal as ClaimsPrincipal;
            var identity = principal?.Identity as ClaimsIdentity;
            var subject = identity?.Claims.SingleOrDefault(x => x.Type == "user_id").Value;
            return subject;
        }

        private SqlGeography MakePolygonValid(DbGeography polygon)
        {
            try
            {
                SqlGeography sqlPolygon = SqlGeography.STGeomFromWKB(new System.Data.SqlTypes.SqlBytes(polygon.AsBinary()), 4326);

                // If the polygon area is larger than an earth hemisphere (510 Trillion m2 / 2), we know it needs to be fixed
                if (polygon.Area.HasValue && polygon.Area.Value > 255000000000000L)
                {
                    // ReorientObject will flip the polygon so the outside becomes the inside
                    sqlPolygon = sqlPolygon.ReorientObject();
                }
                return sqlPolygon;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
