namespace BouvetCodeCamp.Integrasjonstester.Api
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using BouvetCodeCamp.Domene.Entiteter;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    using Should;

    [TestClass]
    public class PostControllerTests : BaseApiTest
    {
        [TestInitialize]
        [TestCleanup]
        public void RyddOppEtterTest()
        {
            SlettPost(TestPostNavn);
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task Get_QueryStringInneholderIngenId_FårListeOverPoster()
        {
            // Arrange
            SørgForAtEnPostFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/post/get";

            IEnumerable<Post> poster;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);

                var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                poster = JsonConvert.DeserializeObject<IEnumerable<Post>>(content);
            }

            // Assert
            poster.ShouldNotBeEmpty();
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task GetPost_QueryStringInneholderId_FårHttpStatusKodeOk()
        {
            // Arrange
            this.SørgForAtEnPostFinnes();
            var alleTestPoster = this.HentAlleTestPoster();
            var testPostDocumentId = alleTestPoster.FirstOrDefault().DocumentId;

            string apiEndPointAddress = ApiBaseAddress + "/api/post/get/" + testPostDocumentId;

            Post post;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);

                var httpResponseMessage = await httpClient.GetAsync(apiEndPointAddress);
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                post = JsonConvert.DeserializeObject<Post>(content);
            }

            // Assert
            post.ShouldNotBeNull();
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task PostPost_GyldigModell_FårHttpStatusKodeOk()
        {
            // Arrange
            const string ApiEndPointAddress = ApiBaseAddress + "/api/post/post";

            bool isSuccessStatusCode;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var modell = new Post { Navn = TestPostNavn };

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
        public async Task PutPost_GyldigModell_FårHttpStatusKodeOk()
        {
            // Arrange
            this.SørgForAtEnPostFinnes();

            var alleTestPoster = this.HentAlleTestPoster();
            var testPost = alleTestPoster.FirstOrDefault();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/post/put";

            bool isSuccessStatusCode;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var modellSomJson = JsonConvert.SerializeObject(testPost);

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
        public void Delete_GyldigModell_AllePosterErSlettet()
        {
            // Arrange
            this.SørgForAtEnPostFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/post/delete";

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);

                var httpResponseMessage = httpClient.DeleteAsync(ApiEndPointAddress).Result;
            }

            // Assert
            var alleTestPoster = this.HentAlleTestPoster();
            alleTestPoster.ShouldBeEmpty();
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public void DeletePost_QueryStringInneholderId_PostenErSlettet()
        {
            // Arrange
            this.SørgForAtEnPostFinnes();

            var alleTestPoster = this.HentAlleTestPoster();
            var testPostDocumentId = alleTestPoster.FirstOrDefault().DocumentId;

            string apiEndPointAddress = ApiBaseAddress + "/api/post/delete/" + testPostDocumentId;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);

                var httpResponseMessage = httpClient.DeleteAsync(apiEndPointAddress).Result;
            }

            // Assert
            alleTestPoster = this.HentAlleTestPoster();
            alleTestPoster.ShouldBeEmpty();
        }
    }
}