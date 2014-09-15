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
        public async Task Opprett_EnAktivitetsloggEntry_AktivitetsloggEntryErLagretIDatabasen()
        {
            // Arrange
            var repository = new AktivitetsloggRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));

            var aktivitetsloggEntry = Builder<AktivitetsloggEntry>.CreateNew()
                .With(o => o.LagId = 1)
                .With(o => o.HendelseType = HendelseType.RegistrertKode)
                .With(o => o.Tid = DateTime.Now)
                .Build();

            // Act
            await repository.Opprett(aktivitetsloggEntry);

            var alleAktivitetsloggEntries = await repository.HentAlle();

            // Assert
            alleAktivitetsloggEntries.Count().ShouldEqual(1);
        }
    }
}