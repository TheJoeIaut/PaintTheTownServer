using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using GeoJSON.Net.Geometry;
using GeoJSON.Net.Contrib.EntityFramework;
using GeoJSON.Net.Feature;
using Newtonsoft.Json;
using PaintTheTownServer.Models;

namespace PaintTheTownServer.Controllers
{
    public class MapController : ApiController
    {
        [HttpPost]
        [ActionName("GetPolysInArea")]
        public async Task<FeatureCollection> GetPolysInArea()
        {
            string json = await Request.Content.ReadAsStringAsync();
            var currentPostion = JsonConvert.DeserializeObject<GeoJSON.Net.Geometry.Point>(json);

            var currentPositionDbGeography = currentPostion.ToDbGeography();

            var result = new FeatureCollection();

            using (var context = new PttContext())
            {
                foreach (var nearArea in context.CoveredAreas.Where(x => x.Geo.Distance(currentPositionDbGeography) < 10000))
                {
                    result.Features.Add(new Feature(nearArea.Geo.ToGeoJSONGeometry()));
                }
                return result;
            }
        }

        [HttpPost]
        [ActionName("SaveArea")]
        public async void SaveArea()
        {
            var json = await Request.Content.ReadAsStringAsync();
            var newPolygon = SqlSpatialFunctions.Reduce(JsonConvert.DeserializeObject<Polygon>(json).ToDbGeography(), 1); 

            using (var context = new PttContext())
            {
                var intersectingAreas = context.CoveredAreas.Where(x => x.Geo.Intersects(newPolygon)).ToList();

                foreach (var intersectingArea in intersectingAreas)
                {
                    intersectingArea.Geo = intersectingArea.Geo.Difference(newPolygon);
                }

                context.CoveredAreas.Add(new CoveredArea()
                {
                    User = context.Users.First(),
                    Geo = newPolygon
                });

                context.SaveChanges();
            }
        }
    }
}
