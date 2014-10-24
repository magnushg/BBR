namespace BouvetCodeCamp.Integrasjonstester.Api
{
    using System.Collections.Generic;
    using System.Linq;
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
    public class LagControllerTests : BaseApiTest
    {
        [TestInitialize]
        [TestCleanup]
        public void RyddOppEtterTest()
        {
            SlettLag(TestLagId);
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task Get_QueryStringInneholderIngenId_FårListeOverLag()
        {
            // Arrange
            SørgForAtEtLagFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/admin/lag/get";

            IEnumerable<Lag> lag;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);

                var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                lag = JsonConvert.DeserializeObject<IEnumerable<Lag>>(content);
            }

            // Assert
            lag.ShouldNotBeEmpty();
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task GetLag_QueryStringInneholderId_FårHttpStatusKodeOk()
        {
            // Arrange
            SørgForAtEtLagFinnes();
            var alleTestLag = await this.HentAlleTestLag();
            var testLagDocumentId = alleTestLag.FirstOrDefault().DocumentId;

            string apiEndPointAddress = ApiBaseAddress + "/api/admin/lag/get/" + testLagDocumentId;

            Lag lag;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);

                var httpResponseMessage = await httpClient.GetAsync(apiEndPointAddress);
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                lag = JsonConvert.DeserializeObject<Lag>(content);
            }

            // Assert
            lag.ShouldNotBeNull();
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task PostLag_GyldigModell_FårHttpStatusKodeOk()
        {
            // Arrange
            const string ApiEndPointAddress = ApiBaseAddress + "/api/admin/lag/post";

            bool isSuccessStatusCode;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var modell = new Lag { LagId = TestLagId };

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
        public async Task PostLag_ModellErEtForStortLagobjekt_BlirLagretMedDatatap()
        {
            // Arrange
            string ApiEndPointAddress = ApiBaseAddress + "/api/admin/lag/post";

            bool isSuccessStatusCode = false;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var tusenHendelser = (List<LoggHendelse>)Builder<LoggHendelse>.CreateListOfSize(2000).All().Build();
                var tusenPifPosisjoner = (List<PifPosisjon>)Builder<PifPosisjon>.CreateListOfSize(2000).All().Build();
                var tusenMeldinger = (List<Melding>)Builder<Melding>.CreateListOfSize(50).All().Build();

                var modell = Builder<Lag>.CreateNew()
                    .With(o => o.LagId = TestLagId)
                    .With(o => o.LoggHendelser = tusenHendelser)
                    .With(o => o.PifPosisjoner = tusenPifPosisjoner)
                    .With(o => o.Meldinger = tusenMeldinger)
                    .Build();

                var modellSomJson = JsonConvert.SerializeObject(modell);

                var httpResponseMessage = await httpClient.PostAsync(
                    ApiEndPointAddress,
                    new StringContent(modellSomJson, Encoding.UTF8, "application/json"));

                isSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
            }

            ApiEndPointAddress = ApiBaseAddress + "/api/admin/lag/get";

            IEnumerable<Lag> lag;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);

                var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                lag = JsonConvert.DeserializeObject<IEnumerable<Lag>>(content);
            }

            var testlagTilOppdatering = lag.First();

            ApiEndPointAddress = ApiBaseAddress + "/api/admin/lag/put";

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var modellSomJson = JsonConvert.SerializeObject(testlagTilOppdatering);

                var httpResponseMessage = await httpClient.PutAsync(
                    ApiEndPointAddress,
                    new StringContent(modellSomJson, Encoding.UTF8, "application/json"));

                isSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
            }

            // Assert
            isSuccessStatusCode.ShouldBeTrue();
        }

        static double ConvertBytesToKilebytes(long bytes)
        {
            return (bytes / 1024f);
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task PutLag_GyldigModell_FårHttpStatusKodeOk()
        {
            // Arrange
            SørgForAtEtLagFinnes();

            var alleTestLag = await this.HentAlleTestLag();
            var testLag = alleTestLag.FirstOrDefault();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/admin/lag/put";

            bool isSuccessStatusCode;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var modellSomJson = JsonConvert.SerializeObject(testLag);

                var httpResponseMessage = await httpClient.PutAsync(
                    ApiEndPointAddress,
                    new StringContent(modellSomJson, Encoding.UTF8, "application/json"));

                isSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
            }

            // Assert
            isSuccessStatusCode.ShouldBeTrue();
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task Delete_GyldigModell_AlleLagErSlettet()
        {
            // Arrange
            SørgForAtEtLagFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/admin/lag/delete";

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);

                var httpResponseMessage = httpClient.DeleteAsync(ApiEndPointAddress).Result;
            }

            // Assert
            var alleTestLag = await this.HentAlleTestLag();
            alleTestLag.ShouldBeEmpty();
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task DeleteLag_QueryStringInneholderId_LagetErSlettet()
        {
            // Arrange
            SørgForAtEtLagFinnes();

            var alleTestLag = await this.HentAlleTestLag();
            var testLagDocumentId = alleTestLag.FirstOrDefault().DocumentId;

            string apiEndPointAddress = ApiBaseAddress + "/api/admin/lag/delete/" + testLagDocumentId;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);

                var httpResponseMessage = httpClient.DeleteAsync(apiEndPointAddress).Result;
            }

            // Assert
            alleTestLag = await this.HentAlleTestLag();
            alleTestLag.ShouldBeEmpty();
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task DeleteByLagId_GyldigModell_LagetErSlettet()
        {
            // Arrange
            SørgForAtEtLagFinnes();

            var alleTestLag = await this.HentAlleTestLag();
            var testLagLagId = alleTestLag.FirstOrDefault().LagId;

            string apiEndPointAddress = ApiBaseAddress + "/api/admin/lag/deletebylagid/" + testLagLagId;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);

                var httpResponseMessage = httpClient.DeleteAsync(apiEndPointAddress).Result;
            }

            // Assert
            alleTestLag = await this.HentAlleTestLag();
            alleTestLag.ShouldBeEmpty();
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task TildelPoeng_GyldigModell_FårHttpStatusKodeOk()
        {
            // Arrange
            SørgForAtEtLagFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/admin/lag/tildelpoeng";

            bool isSuccessStatusCode;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var modell = new PoengInputModell
                                 {
                                     LagId = TestLagId,
                                     Poeng = 10
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
        public async Task OpprettHendelse_GyldigModell_FårHttpStatusKodeOk()
        {
            // Arrange
            SørgForAtEtLagFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/admin/lag/oppretthendelse";

            bool isSuccessStatusCode;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var modell = new LoggHendelseInputModell
                {
                    LagId = TestLagId,
                    Kommentar = "Testkommentar",
                    HendelseType = HendelseType.Achievement
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
    }
}