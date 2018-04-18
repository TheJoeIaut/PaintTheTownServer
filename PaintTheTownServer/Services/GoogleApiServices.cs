using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using PaintTheTownServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaintTheTownServer.Services
{
    public class GoogleApiServices
    {
        public static ClientSecrets Secrets { get; } = new ClientSecrets
        {
            ClientId = "300353354116-bigm93gijg4013dro7qnkg4je67curtg.apps.googleusercontent.com",
            ClientSecret = "WcoDYC0_qDxaxYRd4mcWxKHk"
        };
        public GoogleApiServices()
        {
            Flow = new OfflineAccessGoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = Secrets,
                Scopes = new string[] { "https://www.googleapis.com/auth/games" },
             
            });
        }




        public OfflineAccessGoogleAuthorizationCodeFlow Flow { get; set; } 

        public async System.Threading.Tasks.Task<string> GetRefreshTokenFromAuthCodeAsync(string authCode, User user)
        {
            var token = await Flow.ExchangeCodeForTokenAsync(user.Id, authCode, "https://localhost:44301", new System.Threading.CancellationToken());

            return token.RefreshToken;
        }
    }

    public class OfflineAccessGoogleAuthorizationCodeFlow : GoogleAuthorizationCodeFlow
    {
        public OfflineAccessGoogleAuthorizationCodeFlow(GoogleAuthorizationCodeFlow.Initializer initializer) : base(initializer) { }

        public override AuthorizationCodeRequestUrl CreateAuthorizationCodeRequest(string redirectUri)
        {
            return new GoogleAuthorizationCodeRequestUrl(new Uri(AuthorizationServerUrl))
            {
                ClientId = ClientSecrets.ClientId,               
                Scope = string.Join(" ",Scopes),
                RedirectUri = redirectUri,
                AccessType = "offline",
                ApprovalPrompt = "force"
            };
        }


    };
}