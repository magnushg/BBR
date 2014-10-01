namespace BouvetCodeCamp.Integrasjonstester.Api
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.Domene.InputModels;

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

            string apiEndPointAddress = ApiBaseAddress + "/api/lag/get/" + testLagDocumentId;

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
            const string ApiEndPointAddress = ApiBaseAddress + "/api/lag/post";

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
        public async Task PutLag_GyldigModell_FårHttpStatusKodeOk()
        {
            // Arrange
            SørgForAtEtLagFinnes();

            var alleTestLag = await this.HentAlleTestLag();
            var testLag = alleTestLag.FirstOrDefault();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/lag/put";

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

            const string ApiEndPointAddress = ApiBaseAddress + "/api/lag/delete";

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

            string apiEndPointAddress = ApiBaseAddress + "/api/lag/delete/" + testLagDocumentId;

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

            string apiEndPointAddress = ApiBaseAddress + "/api/lag/deletebylagid/" + testLagLagId;

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
            
            const string ApiEndPointAddress = ApiBaseAddress + "/api/lag/tildelpoeng";

            bool isSuccessStatusCode;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var modell = new PoengModell
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
    }
}