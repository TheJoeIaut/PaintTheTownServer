using PaintTheTownServer.Filter;
using PaintTheTownServer.Helper;
using PaintTheTownServer.Models;
using PaintTheTownServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace PaintTheTownServer.Controllers
{
    [JWTAuthenticationFilter]
    public class AccountController : ApiController
    {
        [HttpPost]
        [ActionName("Auhtorize")]
        public async System.Threading.Tasks.Task Auhtorize([FromBody] string authCode)
        {
            using (var context = new PttContext())
            {
                var user = UserHelper.GetUser(context,RequestContext);
                if (user.RefreshToken == null)
                {
                    var googleapiservices = new GoogleApiServices();

                    user.RefreshToken = await googleapiservices.GetRefreshTokenFromAuthCodeAsync(authCode, user);
                    context.SaveChanges();
                }
            }
        }
    }
}