namespace BouvetCodeCamp.Integrasjonstester.Api
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Bouvet.BouvetBattleRoyale.Domene.OutputModels;

    using BouvetCodeCamp.Domene.Entiteter;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    using Should;

    [TestClass]
    public class InfisertControllerTests : BaseApiTest
    {
        [TestInitialize]
        [TestCleanup]
        public void RyddOppEtterTest()
        {
            SlettPost(TestPostNavn);
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task HentSone_HarEnInfisertPolygon_FårEnInfisertPolygon()
        {
            // Arrange
            SørgForAtEtInfisertPolygonFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/admin/infisert/get";

            InfisertPolygonOutputModell infisertPolygonOutputModell;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader(Brukernavn, Passord);

                var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                infisertPolygonOutputModell = JsonConvert.DeserializeObject<InfisertPolygonOutputModell>(content);
            }

            // Assert
            infisertPolygonOutputModell.Koordinater.ShouldNotBeEmpty();
         }
    }
}