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
    public class MeldingRepositoryIntegrasjonstester : BaseRepositoryIntegrasjonstest
    {
        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Opprett_EnMelding_MeldingErLagretIDatabasen()
        {
            // Arrange
            var repository = new MeldingRepository();

            var melding = Builder<Melding>.CreateNew()
                .With(o => o.LagId = 1)
                .With(o => o.Type = MeldingsType.Lengde)
                .With(o => o.Tekst = "20m")
                .With(o => o.Tid = DateTime.Now)
                .Build();

            // Act
            await repository.Opprett(melding);

            var alleMeldinger = await repository.HentAlle();

            // Assert
            alleMeldinger.Count().ShouldEqual(1);
        }
    }
}