namespace Bouvet.BouvetBattleRoyale.Integrasjonstester.Api
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using Bouvet.BouvetBattleRoyale.Domene;
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Domene.InputModels;
    using Bouvet.BouvetBattleRoyale.Domene.OutputModels;

    using Newtonsoft.Json;

    using NUnit.Framework;

    using Should;

    [TestFixture]
    public class BaseGameControllerTests : BaseApiTest
    {
        [SetUp]
        [TearDown]
        public void SetUp()
        {
            this.SlettLag(TestLagId);
        }

        [Test]
        [Category(Testkategorier.Api)]
        public async Task HentPifPosisjon_GyldigModell_FårHttpStatusKodeOk()
        {
            // Arrange
            var pifPosisjoner = new List<PifPosisjon>
                                    {
                                                        new PifPosisjon {
                                                            LagId = TestLagId,
                                                            Posisjon = new Koordinat
                                                            {
                                                              Latitude = "12.1",
                                                              Longitude = "12.1"
                                                            },
                                                            Tid = DateTime.Now
                                                        }
                                    };

            this.SørgForAtEtLagMedPifPosisjonerFinnes(pifPosisjoner);

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/base/hentpifposisjon/" + TestLagId;

            bool isSuccessStatusCode;

            // Act
            using (var httpClient = new HttpClient())
            {
                var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);

                isSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
            }

            // Assert
            isSuccessStatusCode.ShouldBeTrue();
        }

        [Test]
        [Category(Testkategorier.Api)]
        public async Task HentPifPosisjon_LagetHarEnPifPosisjon_FårPifPosisjon()
        {
            // Arrange
            var pifPosisjoner = new List<PifPosisjon>
                                    {
                                                        new PifPosisjon {
                                                            LagId = TestLagId,
                                                            Posisjon = new Koordinat
                                                            {
                                                              Latitude = "12.1",
                                                              Longitude = "12.1"
                                                            },
                                                            Tid = DateTime.Now
                                                        }
                                    };

            this.SørgForAtEtLagMedPifPosisjonerFinnes(pifPosisjoner);

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/base/hentpifposisjon/" + TestLagId;

            PifPosisjonOutputModell pifPosisjonOutput;

            // Act
            using (var httpClient = new HttpClient())
            {
                var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                pifPosisjonOutput = JsonConvert.DeserializeObject<PifPosisjonOutputModell>(content);
            }

            // Assert
            pifPosisjonOutput.LagId.ShouldNotBeEmpty();
            pifPosisjonOutput.Latitude.ShouldNotBeEmpty();
            pifPosisjonOutput.Longitude.ShouldNotBeEmpty();
            pifPosisjonOutput.Tid.ShouldNotEqual(null);
        }

        [Test]
        [Category(Testkategorier.Api)]
        public async Task HentPifPosisjon_LagetHarIngenPifPosisjoner_FårTomPifPosisjon()
        {
            // Arrange
            SørgForAtEtLagFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/base/hentpifposisjon/" + TestLagId;

            PifPosisjonOutputModell pifPosisjonOutput;

            // Act
            using (var httpClient = new HttpClient())
            {
                var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                pifPosisjonOutput = JsonConvert.DeserializeObject<PifPosisjonOutputModell>(content);
            }

            // Assert
            pifPosisjonOutput.LagId.ShouldEqual(null);
            pifPosisjonOutput.Latitude.ShouldEqual(null);
            pifPosisjonOutput.Longitude.ShouldEqual(null);
            pifPosisjonOutput.Tid.ShouldEqual(null);
        }

        [Test]
        [Category(Testkategorier.Api)]
        public async Task SendPifMelding_GyldigModell_FårHttpStatusCodeCreated()
        {
            // Arrange
            this.SørgForAtEtLagFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/base/sendpifmelding";
            bool isSuccessStatusCode;
            HttpStatusCode responseCode;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var modell = new MeldingInputModell
                {
                    LagId = TestLagId,
                    Innhold = "Heihei",
                    Type = MeldingType.Fritekst
                };

                var modellSomJson = JsonConvert.SerializeObject(modell);

                var httpResponseMessage = await httpClient.PostAsync(
                    ApiEndPointAddress,
                    new StringContent(modellSomJson, Encoding.UTF8, "application/json"));

                isSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
                responseCode = httpResponseMessage.StatusCode;
            }

            // Assert
            isSuccessStatusCode.ShouldBeTrue();
            responseCode.ShouldEqual(HttpStatusCode.OK);
        }

        [Test]
        [Category(Testkategorier.Api)]
        public async Task SendPifMelding_UgyldigModell_FårHttpStatusCodeBadRequest()
        {
            // Arrange
            this.SørgForAtEtLagFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/base/sendpifmelding";
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

        [Test]
        [Category(Testkategorier.Api)]
        public async Task HentRegistrerteKoder_LagHarRegistrerteKoder_FårKoder()
        {
            // Arrange
            this.SørgForAtEtLagMedRegistrerteKoderFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/base/hentregistrertekoder/" + TestLagId;

            IEnumerable<string> koder;

            // Act
            using (var httpClient = new HttpClient())
            {
                var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                koder = JsonConvert.DeserializeObject<IEnumerable<string>>(content);
            }

            // Assert
            koder.ShouldNotBeEmpty();
        }

        [Test]
        [Category(Testkategorier.Api)]
        public async Task HentRegistrerteKoder_LagHarIngenRegistrerteKoder_FårTomt()
        {
            // Arrange
            this.SørgForAtEtLagFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/base/hentregistrertekoder/" + TestLagId;

            IEnumerable<KodeOutputModel> kodeModeller;

            // Act
            using (var httpClient = new HttpClient())
            {
                var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                kodeModeller = JsonConvert.DeserializeObject<IEnumerable<KodeOutputModel>>(content);
            }

            // Assert
            kodeModeller.ShouldBeEmpty();
        }

        [Test]
        [Category(Testkategorier.Api)]
        public async Task HentGjeldendePost_AllePosterErOppdaget_FårNull()
        {
            // Arrange
            var oppdagedePostKoder = new List<LagPost> { new LagPost { PostTilstand = PostTilstand.Oppdaget } };

            SørgForAtEtLagMedLagPostKoderFinnes(oppdagedePostKoder);

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/base/hentgjeldendepost/" + TestLagId;
            
            PostOutputModell postOutputModell;

            // Act
            using (var httpClient = new HttpClient())
            {
                var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                postOutputModell = JsonConvert.DeserializeObject<PostOutputModell>(content);
            }

            // Assert
            postOutputModell.ShouldBeNull();
        }

        [Test]
        [Category(Testkategorier.Api)]
        public async Task HentGjeldendePost_LagHarPosterSomIkkeErOppdaget_FårPost()
        {
            // Arrange
            var oppdagedePostKoder = new List<LagPost> { new LagPost { PostTilstand = PostTilstand.Ukjent } };

            SørgForAtEtLagMedLagPostKoderFinnes(oppdagedePostKoder);

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/base/hentgjeldendepost/" + TestLagId;

            PostOutputModell postOutputModell;

            // Act
            using (var httpClient = new HttpClient())
            {
                var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                postOutputModell = JsonConvert.DeserializeObject<PostOutputModell>(content);
            }

            // Assert
            postOutputModell.ShouldNotBeNull();
        }
    }
}