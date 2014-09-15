using System;
using System.Linq;
using System.Threading.Tasks;
using BouvetCodeCamp.Dataaksess;
using BouvetCodeCamp.Dataaksess.Repositories;
using BouvetCodeCamp.Felles;
using BouvetCodeCamp.Felles.Entiteter;
using BouvetCodeCamp.Felles.Konfigurasjon;
using FizzWare.NBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace BouvetCodeCamp.Integrasjonstester.DataAksess
{
    [TestClass]
    public class LagRepositoryIntegrasjonstester : BaseRepositoryIntegrasjonstest
    {
        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Opprett_EtLag_EtLagErLagret()
        {
            // Arrange
            var repository = new LagRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));

            var melding = Builder<Lag>.CreateNew()
                .With(o => o.LagId = "abc")
                .With(o => o.Poeng = 2)
                .Build();

            // Act
            await repository.Opprett(melding);

            var alleLag = await repository.HentAlle();

            // Assert
            alleLag.Count().ShouldEqual(1);
        }

        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Oppdater_EtLagMedNyLagId_LagetErOppdatertMedNyLagId()
        {
            // Arrange
            var repository = new LagRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));

            const string nyLagId = "cba";

            var aktivitetsloggEntry = Builder<Lag>.CreateNew()
                .With(o => o.LagId = "abc")
                .With(o => o.Poeng = 0)
                .Build();

            var opprettetDocument = await repository.Opprett(aktivitetsloggEntry);

            var hentetDocument = await repository.Hent(opprettetDocument.Id);
            hentetDocument.LagId = nyLagId;

            // Act
            await repository.Oppdater(hentetDocument);

            var oppdatertDocument = await repository.Hent(hentetDocument.Id);

            // Assert
            oppdatertDocument.LagId.ShouldEqual(nyLagId);
        }
    }
}