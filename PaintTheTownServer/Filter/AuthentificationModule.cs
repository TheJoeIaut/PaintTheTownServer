using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using Microsoft.IdentityModel.Tokens;

namespace PaintTheTownServer.Filter
{
    public class AuthenticationModule
    {
        private static SecurityKey[] _keys;

        public ClaimsPrincipal ValidateJwtString(string authToken)
        {
            if (_keys == null)
            {
                if (GetKeys()) return null;
            }

            const string firebaseProjectId = "paintthetown-eb827";
            var parameters = new TokenValidationParameters
            {
                ValidIssuer = "https://securetoken.google.com/" + firebaseProjectId,
                ValidAudience = firebaseProjectId,
                IssuerSigningKeys = _keys,
            };

            var handler = new JwtSecurityTokenHandler();
            SecurityToken token;
            ClaimsPrincipal principal = handler.ValidateToken(authToken, parameters, out token);
            var jwt = (JwtSecurityToken)token;
            if (jwt.Header.Alg != SecurityAlgorithms.RsaSha256)
            {
                throw new SecurityTokenInvalidSignatureException(
                    "The token is not signed with the expected algorithm.");
            }

            var identity = principal.Identity as ClaimsIdentity;
            identity?.AddClaim(new Claim("userSubject", jwt.Subject));

            return principal;
        }

        private static bool GetKeys()
        {
            var client = new HttpClient {BaseAddress = new Uri("https://www.googleapis.com/robot/v1/metadata/")};
            var response = client.GetAsync("x509/securetoken@system.gserviceaccount.com").Result;
            if (!response.IsSuccessStatusCode)
            {
                return true;
            }
            var x509Data = response.Content.ReadAsAsync<Dictionary<string, string>>().Result;
            _keys = x509Data.Values.Select(CreateSecurityKeyFromPublicKey).ToArray();
            return false;
        }

        private static SecurityKey CreateSecurityKeyFromPublicKey(string data)
        {
            return new X509SecurityKey(new X509Certificate2(Encoding.UTF8.GetBytes(data)));
        }
    }
}