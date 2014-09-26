namespace BouvetCodeCamp.Integrasjonstester.Api
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using BouvetCodeCamp.Domene.InputModels;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    using Should;

    [TestClass]
    public class PifGameControllerTests
    {
        private const string Passord = "mysecret";

        private const string Brukernavn = "bouvet";

        const string ApiBaseAddress = "http://bouvetcodecamp";

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task SendPifPosition_GyldigModell_FårHttpStatusKodeOk()
        {
            // Arrange
            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/pif/sendPifPosition";
            bool isSuccessStatusCode;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var modell = new GeoPosisjonModel { LagId = "1", Latitude = "14.02", Longitude = "11" };

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