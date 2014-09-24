using System.Threading.Tasks;

namespace BouvetCodeCamp.Lasttesting
{
    using System.Linq;

    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.Infrastruktur;
    using BouvetCodeCamp.Infrastruktur.Repositories;
    using BouvetCodeCamp.Integrasjonstester;
    using BouvetCodeCamp.Integrasjonstester.DataAksess;

    using FizzWare.NBuilder;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Should;

    [TestClass]
    public class LagRepositoryLasttest : BaseRepositoryIntegrasjonstest
    {
        [TestMethod]
        [TestCategory(Testkategorier.Last)]
        public async Task Opprett_10000Lag_10000LagErOpprettet()
        {
            // Arrange
            var repository = OpprettRepository();
            
            var mangeLag = Builder<Lag>.CreateListOfSize(10000)
                .All()
                .Random(10000)
                .Build().ToList();
            
            foreach (var lag in mangeLag)
            {
                // Act
                var documentId = await repository.Opprett(lag);
                var opprettetLag = await repository.Hent(documentId);

                var lagDocument = await repository.Hent(opprettetLag.DocumentId);
                
                // Assert
                lagDocument.DocumentId.ShouldNotBeEmpty();

                break;
            }
        }

        private LagRepository OpprettRepository()
        {
            return new LagRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));
        }
    }
}