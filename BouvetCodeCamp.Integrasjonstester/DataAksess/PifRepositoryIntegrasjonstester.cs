namespace BouvetCodeCamp.Integrasjonstester.DataAksess
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BouvetCodeCamp.Dataaksess.Repositories;
    using BouvetCodeCamp.Felles.Entiteter;

    using FizzWare.NBuilder;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Should;

    [TestClass]
    public class PifRepositoryIntegrasjonstester : BaseRepositoryIntegrasjonstest
    {
        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Opprett_EnPifPosisjon_PifPosisjonErLagretIDatabasen()
        {
            // Arrange
            var repository = new PifPosisjonRepository();

            var pifPosisjon = Builder<PifPosisjon>.CreateNew()
                    .With(o => o.LagId = 1)
                    .With(o => o.Latitude = "34.1412542")
                    .With(o => o.Longitude = "10.2134124")
                    .With(o => o.Tid = DateTime.Now)
                    .Build();

            // Act
            await repository.Opprett(pifPosisjon);

            var allePifPosisjoner = await repository.HentAlle();

            // Assert
            allePifPosisjoner.Count().ShouldEqual(1);
        }
    }
}