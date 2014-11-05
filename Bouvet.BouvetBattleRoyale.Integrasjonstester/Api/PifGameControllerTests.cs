namespace Bouvet.BouvetBattleRoyale.Integrasjonstester.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using Bouvet.BouvetBattleRoyale.Domene;
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Domene.InputModels;
    using Bouvet.BouvetBattleRoyale.Domene.OutputModels;

    using FizzWare.NBuilder;

    using Newtonsoft.Json;

    using NUnit.Framework;

    using Should;

    [TestFixture]
    public class PifGameControllerTests : BaseApiTest
    {
        [SetUp]
        [TearDown]
        public void RyddOppEtterTest()
        {
            SlettLag(TestLagId);
        }

        [Test]
        [Category(Testkategorier.Api)]
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
        
        private int sekvenstall = 0;

        private int HentSekvenstall()
        {
            return sekvenstall++;
        }

        [Test]
        [Category(Testkategorier.Api)]
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

        [Test]
        [Category(Testkategorier.Api)]
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

            var alleTestLag = await HentAlleTestLag();
            var testLag = alleTestLag.FirstOrDefault();

            // Assert
            isSuccessStatusCode.ShouldBeTrue();
        }

        [Test]
        [Category(Testkategorier.Api)]
        public async Task SendPostKode_GodkjentKodeOgDetteVarSistePost_GirNullSomNestePost()
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

            var gjeldendePost = await HentGjeldendePost(TestLagId);

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/pif/sendpostkode";
            bool isSuccessStatusCode;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var modell = new PostInputModell
                {
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
            
            var alleTestLag = await HentAlleTestLag();
            var testLag = alleTestLag.FirstOrDefault();
            var nyGjeldendePost = await HentGjeldendePost(TestLagId);

            // Assert
            isSuccessStatusCode.ShouldBeTrue();
            gjeldendePost.ShouldNotEqual(nyGjeldendePost);
            nyGjeldendePost.ShouldBeNull();
            testLag.Poeng.ShouldEqual(PoengTildeling.KodeOppdaget);
        }

        private async Task<int?> HentGjeldendePost(string lagId)
        {
            PostOutputModell postOutputModell;

            var ApiEndPointAddress = ApiBaseAddress + "/api/game/base/hentgjeldendepost/" + lagId;

            // Act
            using (var httpClient = new HttpClient())
            {
                var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                postOutputModell = JsonConvert.DeserializeObject<PostOutputModell>(content);
            }

            if (postOutputModell == null) 
                return null;

            return postOutputModell.Nummer;
        }

        [Test]
        [Category(Testkategorier.Api)]
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

        [Test]
        [Category(Testkategorier.Api)]
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

        [Test]
        [Category(Testkategorier.Api)]
        public async Task ErInfisert_PifErInnenforInfisertSone_GirTrue()
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
                                                            Tid = DateTime.Now,
                                                            Infisert = true
                                                        }
                                    };

            SørgForAtEtLagMedPifPosisjonerFinnes(pifPosisjoner);

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
            erInfisert.ShouldBeTrue();
        }

        [Test]
        [Category(Testkategorier.Api)]
        public async Task ErInfisert_PifErIkkeInnenforInfisertSone_GirFalse()
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
                                                            Tid = DateTime.Now,
                                                            Infisert = false
                                                        }
                                    };

            SørgForAtEtLagMedPifPosisjonerFinnes(pifPosisjoner);

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

        [Test]
        [Category(Testkategorier.Api)]
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