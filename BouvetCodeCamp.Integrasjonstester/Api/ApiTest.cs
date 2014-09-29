namespace BouvetCodeCamp.Integrasjonstester.Api
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using BouvetCodeCamp.Domene.Entiteter;

    using FizzWare.NBuilder;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    [TestClass]
    public class ApiTest
    {
        protected const string Passord = "mysecret";

        protected const string Brukernavn = "bouvet";

        protected const string ApiBaseAddress = "http://bouvetcodecamp";

        protected const string LagId = "testlag1";

        [TestInitialize]
        [TestCleanup]
        public void RyddOppEtterTest()
        {
            SlettLag(LagId);
        }

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

        private bool SlettLag(string lagId)
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

        protected void SørgForAtEtLagFinnes()
        {
            var lag = Builder<Lag>.CreateNew()
                .With(o => o.LagId = LagId)
                .Build();

            var lagOpprettet = this.OpprettLagViaApi(lag).Result;

            if (!lagOpprettet)
                Assert.Fail();
        }

        protected void SørgForAtEtLagMedKoderFinnes(List<Kode> koder)
        {
            var lagMedKoder = Builder<Lag>.CreateNew()
                .With(o => o.LagId = LagId)
                .With(o => o.Koder = koder)
                .Build();

            var lagOpprettet = this.OpprettLagViaApi(lagMedKoder).Result;

            if (!lagOpprettet)
                Assert.Fail();
        }
    }
}