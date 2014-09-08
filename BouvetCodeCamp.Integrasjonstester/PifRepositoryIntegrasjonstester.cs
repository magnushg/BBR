using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BouvetCodeCamp.Integrasjonstester
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BouvetCodeCamp.Dataaksess;
    using BouvetCodeCamp.Felles;

    using FizzWare.NBuilder;

    using Microsoft.Azure.Documents.Client;

    using Should;
    
    [TestClass]
    public class PifRepositoryIntegrasjonstester
    {
        private const string DatabaseId = "BouvetCodeCamp2014Integrasjontester";
        private const string CollectionId = "PifPosisjoner";

        [TestInitialize]
        public async void FørHverTest()
        {
            DocumentClient client;

            using (client = new DocumentClient(new Uri(Konstanter.DocumentDbEndpoint), Konstanter.DocumentDbAuthKey))
            {
                await DocumentDbHelpers.HentEllerOpprettDatabaseAsync(client, DatabaseId);
            }
        }

        [TestCleanup]
        public async void EtterHverTest()
        {
            DocumentClient client;

            using (client = new DocumentClient(new Uri(Konstanter.DocumentDbEndpoint), Konstanter.DocumentDbAuthKey))
            {
                await DocumentDbHelpers.SlettDatabaseAsync(client, DatabaseId);
            }
        }

        [TestMethod]
        public async Task Opprett_EnPifPosisjon_PifPosisjonErLagretIDatabasen()
        {
            // Arrange
            var repository = new PifPosisjonRepository(DatabaseId, CollectionId);
            var pifPosisjon = Builder<PifPosisjon>.CreateNew()
                    .With(o => o.LagId = 1)
                    .With(o => o.Latitude = "34.1412542")
                    .With(o => o.Longitude = "10.2134124")
                    .Build();

            // Act
            await repository.Opprett(pifPosisjon);

            var allePifPosisjoner = await repository.HentAlle();

            // Assert
            allePifPosisjoner.Count().ShouldEqual(1);
        }
    }
}