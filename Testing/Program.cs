using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Discovery API Sample");
            Console.WriteLine("====================");
            try
            {
                new Program().Run().Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("ERROR: " + e.Message);
                }
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private async Task Run()
        {
 //           var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
 //   new ClientSecrets
 //   {
 //       ClientId = "300353354116-bigm93gijg4013dro7qnkg4je67curtg.apps.googleusercontent.com",
 //       ClientSecret = "WcoDYC0_qDxaxYRd4mcWxKHk"
 //   },
 //   new[] { Google.Apis.Games.v1.GamesService.Scope.Games },
 //   "user"   ,
 //   new System.Threading.CancellationToken()
 //);

            var ctoken = new System.Threading.CancellationToken();

            var clientSecrets = new ClientSecrets
            {
                ClientId = "300353354116-bigm93gijg4013dro7qnkg4je67curtg.apps.googleusercontent.com",
                ClientSecret = "WcoDYC0_qDxaxYRd4mcWxKHk"
            };

            var flow = new AuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = clientSecrets,
                Scopes = new string[] { "https://www.googleapis.com/auth/games" }

            });

            var token = await flow.ExchangeCodeForTokenAsync("112828434304919930917", "4/AAA32YZmmJhZPLS4A07_EMyARC23WQO5SeIduqgJx52C3CEYK6Cp9ePpO_qHeH4Rx0saT4EWL-vbV5Ei5l0ajy4", "https://localhost:44301", ctoken);
            // Create the service.

            var credentials = new UserCredential(new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer    {        ClientSecrets = clientSecrets    }), "user", token);


            var service = new Google.Apis.Games.v1.GamesService(new Google.Apis.Services.BaseClientService.Initializer
            {
                HttpClientInitializer = credentials,
                ApplicationName = "PaintTheTown",
                ApiKey = "AIzaSyCeCYL0Z6mw-PcUA1dzd4H7HxcFFN-vuZ4",
            });

            var result = service.Achievements.Increment("CgkIhPPR894IEAIQAA", 1);

           // GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync("eyJhbGciOiJSUzI1NiIsImtpZCI6ImMwNmEyMTQ5YjdmOTU3MjgwNTJhOTg1YWRlY2JmNWRlMDQ3Y2RhNmYifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vcGFpbnR0aGV0b3duLWViODI3IiwibmFtZSI6IkrDvHJnZW4gU2Now7ZuZXIiLCJwaWN0dXJlIjoiaHR0cHM6Ly9saDMuZ29vZ2xldXNlcmNvbnRlbnQuY29tLy1idUFiRGluaWotMC9BQUFBQUFBQUFBSS9BQUFBQUFBQUFBQS9BRmlZb2YzaW5PRmR3WXpoQlFfeThEU2dTTnVuNDBPRW53L3M5Ni1jL3Bob3RvLmpwZyIsImF1ZCI6InBhaW50dGhldG93bi1lYjgyNyIsImF1dGhfdGltZSI6MTUyMTA1NzM3MiwidXNlcl9pZCI6IjRBS0g4V2pxMUxVNWRxV2ZZR2h4Z01uY3U2UjIiLCJzdWIiOiI0QUtIOFdqcTFMVTVkcVdmWUdoeGdNbmN1NlIyIiwiaWF0IjoxNTIzMjk2NjUwLCJleHAiOjE1MjMzMDAyNTAsImVtYWlsIjoidGhlam9laWF1dEBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiZmlyZWJhc2UiOnsiaWRlbnRpdGllcyI6eyJnb29nbGUuY29tIjpbIjExMjgyODQzNDMwNDkxOTkzMDkxNyJdLCJlbWFpbCI6WyJ0aGVqb2VpYXV0QGdtYWlsLmNvbSJdfSwic2lnbl9pbl9wcm92aWRlciI6Imdvb2dsZS5jb20ifX0.NvUN24H6jdO31lv1DLx8l-APoHSD6rq7gwCVIKoaQIgxi1FZur3XvJDAG2gELZCF_wrV4SiA2zpX-raKtmVjXb-JR7CecZIBQ4Ryi7zYW3h7jiTOKn75_Nn4ZqxOaz5xHSTKXwYwP21TeZB5NYcU1Bu5x4GAL2AN7XIV8Iby21t3Qx9Y7lc9cMdtXlRYs1oJ9Q8vbRPQ6La-W0lJYbPZtj8RYC60Nvaz43ckrJ3Vo1z4gNfVhNb_9Uz9aSfPPAFIelyP0ZJ4ucxH75PsfmC64zNPgOSbhN_P8z34NHSd7MMKkFAkYb4dbqYsQV4NxOSA2SCwqQvvWWW88mnBqLHaIA",null,false);

            // Run the request.
            Console.WriteLine("Executing a list request...");
            result.Execute();
            var asfsa = service.Achievements.List("112828434304919930917");


        }
    }
    
}
