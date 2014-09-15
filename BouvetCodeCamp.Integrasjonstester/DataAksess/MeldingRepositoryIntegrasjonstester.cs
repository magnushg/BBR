using BouvetCodeCamp.Dataaksess;
using BouvetCodeCamp.Felles.Konfigurasjon;

namespace BouvetCodeCamp.Integrasjonstester.DataAksess
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Dataaksess.Repositories;
    using Felles;
    using Felles.Entiteter;

    using FizzWare.NBuilder;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Should;

    [TestClass]
    public class MeldingRepositoryIntegrasjonstester : BaseRepositoryIntegrasjonstest
    {
        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Opprett_EnMelding_MeldingErLagretIDatabasen()
        {
            // Arrange
            var repository = new MeldingRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));
            
            var melding = Builder<Melding>.CreateNew()
                .With(o => o.LagId = "abc")
                .With(o => o.Type = MeldingType.Lengde)
                .With(o => o.Tekst = "20m")
                .With(o => o.Tid = DateTime.Now)
                .Build();

            // Act
            await repository.Opprett(melding);

            var alleMeldinger = await repository.HentAlle();

            // Assert
            alleMeldinger.Count().ShouldEqual(1);
        }

        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Oppdater_EnMeldingMedNyLagId_MeldingErOppdatertMedNyLagId()
        {
            // Arrange
            var repository = new MeldingRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));

            const string nyLagId = "cba";

            var aktivitetsloggEntry = Builder<Melding>.CreateNew()
                .With(o => o.LagId = "abc")
                .With(o => o.Tekst = "test")
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