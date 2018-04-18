using PaintTheTownServer.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Routing;

namespace PaintTheTownServer.Helper
{
    public static class UserHelper
    {
        public static User GetUser(PttContext context, System.Web.Http.Controllers.HttpRequestContext requestContext)
        {
            var subject = GetUserSubject(requestContext);

            if (!context.Users.Any(x => x.Id == subject))
            {
                context.Users.Add(new User
                {
                    Id = subject,
                    Name = ""
                });
            }
            context.SaveChanges();
            var user = context.Users.FirstOrDefault(x => x.Id == subject);
            return user;
        }

        private static string GetUserSubject(System.Web.Http.Controllers.HttpRequestContext requestContext)
        {
            
               var principal = requestContext.Principal as ClaimsPrincipal;
            var identity = principal?.Identity as ClaimsIdentity;
            var subject = identity.FindFirst(JwtRegisteredClaimNames.Sub).Value;
            return subject;
        }
    }
}