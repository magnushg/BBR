namespace BouvetCodeCamp.Integrasjonstester.Api
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using BouvetCodeCamp.Domene.Entiteter;

    using FizzWare.NBuilder;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    [TestClass]
    public class BaseApiTest
    {
        protected const string Passord = "mysecret";

        protected const string Brukernavn = "bouvet";

        protected const string ApiBaseAddress = "http://bouvetcodecamp";

        protected const string TestLagId = "testlag1";

        protected const string TestPostNavn = "testpost1";
        
        protected async Task<bool> OpprettLagViaApi(Lag lag)
        {
            const string ApiEndPointAddress = ApiBaseAddress + "/api/lag/post";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var modellSomJson = JsonConvert.SerializeObject(lag);

                var httpResponseMessage = await httpClient.PostAsync(
                    ApiEndPointAddress,
                    new StringContent(modellSomJson, Encoding.UTF8, "application/json"));

                return httpResponseMessage.IsSuccessStatusCode;
            }
        }

        protected async Task<bool> OpprettPostViaApi(Post post)
        {
            const string ApiEndPointAddress = ApiBaseAddress + "/api/post/post";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var modellSomJson = JsonConvert.SerializeObject(post);

                var httpResponseMessage = await httpClient.PostAsync(
                    ApiEndPointAddress,
                    new StringContent(modellSomJson, Encoding.UTF8, "application/json"));

                return httpResponseMessage.IsSuccessStatusCode;
            }
        }

        protected async Task<bool> OpprettGameStateViaApi(GameState gameState)
        {
            const string ApiEndPointAddress = ApiBaseAddress + "/api/gamestate/post";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var modellSomJson = JsonConvert.SerializeObject(gameState);

                var httpResponseMessage = await httpClient.PostAsync(
                    ApiEndPointAddress,
                    new StringContent(modellSomJson, Encoding.UTF8, "application/json"));

                return httpResponseMessage.IsSuccessStatusCode;
            }
        }

        protected bool SlettLag(string lagId)
        {
            var ApiEndPointAddress = ApiBaseAddress + "/api/lag/deletebylagid/" + lagId;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var httpResponseMessage = httpClient.DeleteAsync(ApiEndPointAddress).Result;

                return httpResponseMessage.IsSuccessStatusCode;
            }
        }

        protected void SlettPost(string postNavn)
        {
            var testPoster = this.HentAlleTestPoster();

            foreach (var post in testPoster)
            {
                var apiEndPointAddress = ApiBaseAddress + "/api/post/delete/" + post.DocumentId;

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var httpResponseMessage = httpClient.DeleteAsync(apiEndPointAddress).Result;
                }
            }
        }

        protected void SørgForAtEtLagFinnes()
        {
            var lag = Builder<Lag>.CreateNew()
                .With(o => o.LagId = TestLagId)
                .Build();

            var lagOpprettet = this.OpprettLagViaApi(lag).Result;

            if (!lagOpprettet)
                Assert.Fail();
        }

        protected void SørgForAtEnPostFinnes()
        {
            var post = Builder<Post>.CreateNew()
                .With(o => o.Navn = TestPostNavn)
                .Build();

            var postOpprettet = this.OpprettPostViaApi(post).Result;

            if (!postOpprettet)
                Assert.Fail();
        }

        protected void SørgForAtEtLagMedLagPostKoderFinnes(List<LagPost> koder)
        {
            var lagMedKoder = Builder<Lag>.CreateNew()
                .With(o => o.LagId = TestLagId)
                .With(o => o.Poster = koder)
                .Build();

            var lagOpprettet = this.OpprettLagViaApi(lagMedKoder).Result;

            if (!lagOpprettet)
                Assert.Fail();
        }

        protected void SørgForAtEtInfisertPolygonFinnes()
        {
            var gameState = Builder<GameState>.CreateNew()
                .With(o => o.InfisertPolygon = new InfisertPolygon
                                                   {
                                                       Koordinater = new[]
                                                                         {
                                                                             new Koordinat("12", "32")
                                                                         }
                                                   })
                .Build();

            var gameStateOpprettet = OpprettGameStateViaApi(gameState).Result;

            if (!gameStateOpprettet)
                Assert.Fail();
        }

        protected async Task<IEnumerable<Lag>> HentAlleTestLag()
        {
            const string ApiEndPointAddress = ApiBaseAddress + "/api/lag/get";

            IEnumerable<Lag> lag;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);

                var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                lag = JsonConvert.DeserializeObject<IEnumerable<Lag>>(content);
            }

            return lag.Where(o => o.LagId == TestLagId);
        }

        protected IEnumerable<Post> HentAlleTestPoster()
        {
            const string ApiEndPointAddress = ApiBaseAddress + "/api/post/get";

            IEnumerable<Post> poster;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);

                var httpResponseMessage = httpClient.GetAsync(ApiEndPointAddress).Result;
                var content = httpResponseMessage.Content.ReadAsStringAsync().Result;

                poster = JsonConvert.DeserializeObject<IEnumerable<Post>>(content);
            }

            return poster.Where(o => o.Navn == TestPostNavn);
        }
    }
}