using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace PaintTheTownServer.Filter
{
    public class JWTAuthenticationFilter : AuthorizationFilterAttribute
    {

        public override void OnAuthorization(HttpActionContext filterContext)
        {
            if (!IsUserAuthorized(filterContext))
            {
                ShowAuthenticationError(filterContext);
                return;
            }
            base.OnAuthorization(filterContext);
        }

        public bool IsUserAuthorized(HttpActionContext actionContext)
        {
            var authHeader = FetchFromHeader(actionContext);

            if (authHeader == null)
                return false;

            var auth = new AuthenticationModule();
            var principal= auth.ValidateJwtString(authHeader);

            if (principal == null)
                return false;

            Thread.CurrentPrincipal = principal;
            return true;
        }

        private static string FetchFromHeader(HttpActionContext actionContext)
        {
            string requestToken = null;

            var authRequest = actionContext.Request.Headers.Authorization;
            if (authRequest != null)
            {
                requestToken = authRequest.Parameter;
            }

            return requestToken;
        }

        private static void ShowAuthenticationError(HttpActionContext filterContext)
        { 
            filterContext.Response =filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
        }
    }
}