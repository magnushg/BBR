namespace Bouvet.BouvetBattleRoyale.Integrasjonstester.Api
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Bouvet.BouvetBattleRoyale.Domene.OutputModels;

    using BouvetCodeCamp.Integrasjonstester;

    using Newtonsoft.Json;

    using NUnit.Framework;

    using Should;

    [TestFixture]
    public class InfisertControllerTests : BaseApiTest
    {
        [SetUp]
        [TearDown]
        public void RyddOppEtterTest()
        {
            SlettPost(TestPostNavn);
        }

        [Test]
        [Category(Testkategorier.Api)]
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