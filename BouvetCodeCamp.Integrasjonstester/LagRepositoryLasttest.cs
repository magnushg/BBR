using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BouvetCodeCamp.Integrasjonstester
{
    using System.Diagnostics;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Web.Http;

    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.Integrasjonstester.DataAksess;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    using Should;

    [TestClass]
    public class LagRepositoryLasttest : BaseRepositoryIntegrasjonstest
    {
        private const string Passord = "mysecret";

        private const string Brukernavn = "bouvet";

        const string ApiBaseAddress = "http://localhost:2014/";

        [TestInitialize]
        public void RyddEtterTest()
        {
            SlettAlleLag();
        }

        [TestMethod]
        [TestCategory(Testkategorier.Last)]
        public async Task Lasttest_api_lag_post()
        {
            // Arrange
            const int AntallTester = 5;

            // Act
            this.KjørTest(AntallTester);

            // Assert
            var antallLag = await this.ValiderResultat();

            antallLag.ShouldEqual(await TestManager.RetryUntilSuccessOrTimeout(async () =>
                await this.ValiderResultat(),
                TimeSpan.FromSeconds(10),
                AntallTester));
        }

        private async Task<int> ValiderResultat()
        {
            const string ApiEndPointAddress = ApiBaseAddress + "/api/lag/get";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader("bouvet", "mysecret");

                var httpResponseMessage = httpClient.GetAsync(ApiEndPointAddress).Result;
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                try
                {
                    var alleLag = JsonConvert.DeserializeObject<IEnumerable<Lag>>(content);

                    return alleLag.Count();
                }
                catch (Exception e)
                {
                    var errorMessage = JsonConvert.DeserializeObject<HttpError>(content);

                    Debug.WriteLine(errorMessage["message"]);
                }
            }

            return 0;
        }

        private void KjørTest(int antallTester)
        {
            for (int i = 0; i < antallTester; i++)
            {
                this.OpprettLag();
            }

            Thread.Sleep(10000);
        }

        private void SlettAlleLag()
        {
            const string ApiEndPointAddress = ApiBaseAddress + "/api/lag/delete";
            var basicAuthorizationHeader = TestManager.OpprettBasicHeader(Brukernavn, Passord);

            using (var webClient = new WebClient())
            {
                webClient.Headers.Add(HttpRequestHeader.Authorization, basicAuthorizationHeader.Scheme + " " + basicAuthorizationHeader.Parameter);

                webClient.UploadStringAsync(new Uri(ApiEndPointAddress), "DELETE", string.Empty);
            }
        }

        private void OpprettLag()
        {
            const string ApiEndPointAddress = ApiBaseAddress + "/api/lag/post";
            var basicAuthorizationHeader = TestManager.OpprettBasicHeader(Brukernavn, Passord);

            using (var webClient = new WebClient())
            {
                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                webClient.Headers.Add(HttpRequestHeader.Authorization, basicAuthorizationHeader.Scheme + " " + basicAuthorizationHeader.Parameter);

                var lagSomJson = "{ \"lagId\" : \"" + 888 + "\" }";

                webClient.UploadStringAsync(new Uri(ApiEndPointAddress), "POST", lagSomJson);
            }
        }
    }
}
