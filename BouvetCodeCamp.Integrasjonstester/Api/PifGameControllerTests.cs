namespace BouvetCodeCamp.Integrasjonstester.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using BouvetCodeCamp.Domene.OutputModels;

    using Domene;
    using Domene.Entiteter;
    using Domene.InputModels;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    using Should;

    [TestClass]
    public class PifGameControllerTests : BaseApiTest
    {
        [TestInitialize]
        [TestCleanup]
        public void RyddOppEtterTest()
        {
            SlettLag(TestLagId);
        }

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

                var modell = new PifPosisjonInputModell { 
                    LagId = TestLagId, 
                    Posisjon = new Koordinat {
                        Latitude = "14.02",
                        Longitude = "11"
                    }
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
                                        PostTilstand = PostTilstand.Ukjent,
                                        Nummer = 1
                                    }
                            };

            SørgForAtEtLagMedLagPostKoderFinnes(koder);

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/pif/sendpostkode";
            bool isSuccessStatusCode;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var modell = new PostInputModell {
                    Kode = PostKode,
                    Koordinat = new Koordinat("12", "12"),
                    LagId = TestLagId,
                    Postnummer = 1
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

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task ErInfisert_FinnesIngenPifPosisjonerForLag_GirFalse()
        {
            // Arrange
            SørgForAtEtLagFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/pif/erinfisert/" + TestLagId;

            bool erInfisert;

            // Act
            using (var httpClient = new HttpClient())
            {
                var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                erInfisert = JsonConvert.DeserializeObject<bool>(content);
            }

            // Assert
            erInfisert.ShouldBeFalse();
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task ErInfisert_PifErInnenforInfisertSone_GirTrue()
        {
            //TODO
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task ErInfisert_PifErIkkeInnenforInfisertSone_GirFalse()
        {
            //TODO
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task HentMeldinger_LagetHarMeldinger_GirMeldinger()
        {
            // Arrange
            var meldinger = new List<Melding>
                                {
                                    new Melding {
                                            LagId = string.Empty,
                                            Tekst = "heihei",
                                            Type = MeldingType.Fritekst,
                                            Tid = DateTime.Now
                                        }
                                };

            SørgForAtEtLagMedMeldingerFinnes(meldinger);

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/pif/hentmeldinger/" + TestLagId;

            IEnumerable<MeldingOutputModell> lagMeldinger;

            // Act
            using (var httpClient = new HttpClient())
            {
                var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                lagMeldinger = JsonConvert.DeserializeObject<IEnumerable<MeldingOutputModell>>(content);
            }

            // Assert
            lagMeldinger.ShouldNotBeEmpty();
        }
    }
}