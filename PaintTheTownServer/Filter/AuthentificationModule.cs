using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;

namespace PaintTheTownServer.Filter
{
    public class AuthenticationModule
    {
        private static SecurityKey[] _keys;

        public async System.Threading.Tasks.Task<ClaimsPrincipal> ValidateJwtStringAsync(string authToken)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(authToken);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, payload.Name),
                    new Claim(ClaimTypes.Name, payload.Name),
                    new Claim(JwtRegisteredClaimNames.FamilyName, payload.FamilyName),
                    new Claim(JwtRegisteredClaimNames.GivenName, payload.GivenName),
                    new Claim(JwtRegisteredClaimNames.Email, payload.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, payload.Subject),
                    new Claim(JwtRegisteredClaimNames.Iss, payload.Issuer),
                };

                var principal = new ClaimsPrincipal();
                principal.AddIdentity(new ClaimsIdentity(claims));
                return principal;
            }
            catch(Exception e)
            {
                throw;
            }

         
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