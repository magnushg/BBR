namespace BouvetCodeCamp.Integrasjonstester.Api
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using BouvetCodeCamp.Domene;
    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.Domene.InputModels;

    using FizzWare.NBuilder;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    using Should;

    [TestClass]
    public class PifGameControllerTests
    {
        private const string Passord = "mysecret";

        private const string Brukernavn = "bouvet";

        const string ApiBaseAddress = "http://bouvetcodecamp";

        private const string LagId = "testlag1";

        [TestCleanup]
        public void RyddOppEtterTest()
        {
            SlettAlleLag();
        }
        
        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task SendPifPosition_GyldigModell_FårHttpStatusKodeOk()
        {
            // Arrange
            SørgForAtEtLagFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/pif/sendPifPosition";

            bool isSuccessStatusCode;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var modell = new GeoPosisjonModel { LagId = LagId, Latitude = "14.02", Longitude = "11" };

                var modellSomJson = JsonConvert.SerializeObject(modell);

                var httpResponseMessage = await httpClient.PostAsync(
                    ApiEndPointAddress, 
                    new StringContent(modellSomJson, Encoding.UTF8, "application/json"));

                isSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
            }

            // Assert
            isSuccessStatusCode.ShouldBeTrue();
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task SendPifPosisjon_UgyldigModell_FårHttpStatusCodeBadRequest()
        {
            // Arrange
            SørgForAtEtLagFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/pif/sendPifPosition";

            bool isSuccessStatusCode;

            HttpStatusCode responseCode;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var modellSomJson = string.Empty;

                var httpResponseMessage = await httpClient.PostAsync(
                    ApiEndPointAddress,
                    new StringContent(modellSomJson, Encoding.UTF8, "application/json"));

                isSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
                responseCode = httpResponseMessage.StatusCode;
            }

            // Assert
            isSuccessStatusCode.ShouldBeFalse();
            responseCode.ShouldEqual(HttpStatusCode.BadRequest);
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task SendPostKode_GyldigModell_FårHttpStatusKodeOk()
        {
            // Arrange
            const string PostKode = "asflææø12";
            var koder = new List<Kode>
                            {
                                new Kode {
                                        Bokstav = PostKode,
                                        Posisjon = new Coordinate("12", "12"),
                                        PosisjonTilstand = PosisjonTilstand.Ukjent
                                    }
                            };

            SørgForAtEtLagMedKoderFinnes(koder);

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/pif/sendpostkode";
            bool isSuccessStatusCode;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var modell = new KodeModel {
                    Kode = PostKode,
                    Koordinat = new Coordinate("12", "12"),
                    LagId = LagId
                };

                var modellSomJson = JsonConvert.SerializeObject(modell);

                var httpResponseMessage = await httpClient.PostAsync(
                    ApiEndPointAddress,
                    new StringContent(modellSomJson, Encoding.UTF8, "application/json"));

                isSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
            }

            // Assert
            isSuccessStatusCode.ShouldBeTrue();
        }

        private void SørgForAtEtLagFinnes()
        {
            var lag = Builder<Lag>.CreateNew()
                .With(o => o.LagId = LagId)
                .Build();

            var lagOpprettet = this.OpprettLagViaApi(lag).Result;

            if (!lagOpprettet)
                Assert.Fail();
        }

        private async Task<bool> OpprettLagViaApi(Lag lag)
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

        private void SørgForAtEtLagMedKoderFinnes(List<Kode> koder)
        {
            var lagMedKoder = Builder<Lag>.CreateNew()
                .With(o => o.LagId = LagId)
                .With(o => o.Koder = koder)
                .Build();

            var lagOpprettet = this.OpprettLagViaApi(lagMedKoder).Result;
            
            if (!lagOpprettet)
                Assert.Fail();
        }

        private bool SlettAlleLag()
        {
            const string ApiEndPointAddress = ApiBaseAddress + "/api/lag/delete";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var httpResponseMessage = httpClient.DeleteAsync(ApiEndPointAddress).Result;

                return httpResponseMessage.IsSuccessStatusCode;
            }
        }
    }
}