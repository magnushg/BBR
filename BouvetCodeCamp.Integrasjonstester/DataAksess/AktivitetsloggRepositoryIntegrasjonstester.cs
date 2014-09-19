using BouvetCodeCamp.Dataaksess;
using BouvetCodeCamp.Felles.Konfigurasjon;

namespace BouvetCodeCamp.Integrasjonstester.DataAksess
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BouvetCodeCamp.Dataaksess.Repositories;
    using BouvetCodeCamp.Felles;
    using BouvetCodeCamp.Felles.Entiteter;

    using FizzWare.NBuilder;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Should;

    [TestClass]
    public class AktivitetsloggRepositoryIntegrasjonstester : BaseRepositoryIntegrasjonstest
    {
        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Opprett_EnAktivitetsloggEntry_AktivitetsloggEntryErLagret()
        {
            // Arrange
            var repository = new AktivitetsloggRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));

            var aktivitetsloggEntry = Builder<AktivitetsloggHendelse>.CreateNew()
                .With(o => o.LagId = "abc")
                .With(o => o.HendelseType = HendelseType.RegistrertKode)
                .With(o => o.Tid = DateTime.Now)
                .Build();

            // Act
            await repository.Opprett(aktivitetsloggEntry);

            var alleAktivitetsloggEntries = await repository.HentAlle();

            // Assert
            alleAktivitetsloggEntries.Count().ShouldEqual(1);
        }

        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Oppdater_EnAktivitetsloggEntryMedNyLagId_AktivitetsloggEntryErOppdatertMedNyLagId()
        {
            // Arrange
            var repository = new AktivitetsloggRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));

            const string nyLagId = "cba";

            var aktivitetsloggEntry = Builder<AktivitetsloggHendelse>.CreateNew()
                .With(o => o.LagId = "abc")
                .With(o => o.HendelseType = HendelseType.RegistrertKode)
                .With(o => o.Tid = DateTime.Now)
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