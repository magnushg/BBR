namespace BouvetCodeCamp.Integrasjonstester.Api
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using BouvetCodeCamp.Domene;
    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.Domene.InputModels;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    using Should;

    [TestClass]
    public class PifGameControllerTests : ApiTest
    {
        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task SendPifPosition_GyldigModell_FårHttpStatusKodeOk()
        {
            // Arrange
            SørgForAtEtLagFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/pif/sendpifposisjon";

            bool isSuccessStatusCode;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var modell = new PifPosisjonModell { LagId = LagId, Latitude = "14.02", Longitude = "11" };

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
            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/pif/sendpifposisjon";

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
            var koder = new List<LagPost>
                            {
                                new LagPost {
                                        Kode = PostKode,
                                        Posisjon = new Koordinat("12", "12"),
                                        PostTilstand = PostTilstand.Ukjent
                                    }
                            };

            SørgForAtEtLagMedKoderFinnes(koder);

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/pif/sendpostkode";
            bool isSuccessStatusCode;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var modell = new KodeModell {
                    Kode = PostKode,
                    Koordinat = new Koordinat("12", "12"),
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

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task SendPostKode_UgyldigModell_FårHttpStatusKodeBadRequest()
        {
            // Arrange
            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/pif/sendpostkode";

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
    }
}