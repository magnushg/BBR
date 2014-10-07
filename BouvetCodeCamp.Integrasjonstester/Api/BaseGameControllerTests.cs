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

    using BouvetCodeCamp.Domene;
    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.Domene.InputModels;
    using BouvetCodeCamp.Domene.OutputModels;

    using FizzWare.NBuilder;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    using Should;

    [TestClass]
    public class BaseGameControllerTests : BaseApiTest
    {
        [TestInitialize]
        [TestCleanup]
        public void RyddOppEtterTest()
        {
            this.SlettLag(TestLagId);
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task HentPifPosisjon_GyldigModell_FårHttpStatusKodeOk()
        {
            // Arrange
            this.SørgForAtEtLagMedEnPifPosisjonFinnes();

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

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task HentPifPosisjon_LagetHarEnPifPosisjon_FårPifPosisjon()
        {
            // Arrange
            this.SørgForAtEtLagMedEnPifPosisjonFinnes();

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

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task HentPifPosisjon_LagetHarIngenPifPosisjoner_FårTomPifPosisjon()
        {
            // Arrange
            this.SørgForAtEtLagMedUtenPifPosisjonFinnes();

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

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
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
                    Tekst = "Heihei",
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

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
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

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task HentRegistrerteKoder_LagHarRegistrerteKoder_FårKoder()
        {
            // Arrange
            this.SørgForAtEtLagMedRegistrerteKoderFinnes();

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
            kodeModeller.ShouldNotBeEmpty();
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
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
    }
}