using BouvetCodeCamp.Dataaksess;
using BouvetCodeCamp.Felles.Konfigurasjon;

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
            var repository = new PifPosisjonRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));

            var pifPosisjon = Builder<PifPosisjon>.CreateNew()
                    .With(o => o.LagId = string.Empty)
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

        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task HentPifPosisjonerForLag_LagetHarPifPosisjoner_GirEnListeMedPosisjoner()
        {
            // Arrange
            var repository = new PifPosisjonRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));
            
            const string lagId = "abc";

            var pifPosisjoner = Builder<PifPosisjon>.CreateListOfSize(5)
                .All()
                .With(o => o.LagId = lagId)
                .Random(5)
                .Build();

            foreach (var pifPosisjon in pifPosisjoner)
            {
                await repository.Opprett(pifPosisjon);
            }

            // Act
            var pifPosisjonerForLag = await repository.HentPifPosisjonerForLag(lagId);

            // Assert
            pifPosisjonerForLag.Count().ShouldEqual(5);
        }

        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task HentPifPosisjonerForLag_LagetHarPifPosisjoner_NyligstePosisjonErFørstIListen()
        {
            // Arrange
            var repository = new PifPosisjonRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));

            const string lagId = "abc";

            var pifPosisjoner = Builder<PifPosisjon>.CreateListOfSize(5)
                .All()
                .With(o => o.LagId = lagId)
                .Random(5)
                .Build();

            foreach (var pifPosisjon in pifPosisjoner)
            {
                await repository.Opprett(pifPosisjon);
            }

            // Act
            var pifPosisjonerForLag = await repository.HentPifPosisjonerForLag(lagId);

            // Assert
            pifPosisjonerForLag.FirstOrDefault().Tid.ShouldBeGreaterThan(pifPosisjonerForLag.LastOrDefault().Tid);
        }
        
        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Oppdater_EnMeldingMedNyTekst_MeldingErOppdatertMedNyTekst()
        {
            // Arrange
            var repository = new PifPosisjonRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));

            const string nyLagId = "cba";

            var aktivitetsloggEntry = Builder<PifPosisjon>.CreateNew()
                .With(o => o.LagId = "abc")
                .With(o => o.LagId = "test")
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