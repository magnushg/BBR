using System.Threading.Tasks;
using BouvetCodeCamp.Dataaksess;
using BouvetCodeCamp.Dataaksess.Repositories;
using BouvetCodeCamp.Felles.Entiteter;
using BouvetCodeCamp.Felles.Konfigurasjon;
using FizzWare.NBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace BouvetCodeCamp.Integrasjonstester.DataAksess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BouvetCodeCamp.Felles;

    [TestClass]
    public class LagRepositoryIntegrasjonstester : BaseRepositoryIntegrasjonstest
    {
        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Hent_LagMed10Poeng_LagHar10Poeng()
        {
            // Arrange
            var repository = OpprettRepository();

            const int Poeng = 10;

            var melding = Builder<Lag>.CreateNew()
                .With(o => o.Poeng = Poeng)
                .Build();

            var document = await repository.Opprett(melding);

            // Act
            var lagretLag = await repository.Hent(document.Id);

            // Assert
            lagretLag.Poeng.ShouldEqual(Poeng);
        }

        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Hent_LagMedKode_LagHarKode()
        {
            // Arrange
            var repository = OpprettRepository();

            var kode = new Kode { PosisjonTilstand = PosisjonTilstand.Oppdaget, Bokstav = "a", Gps = new Coordinate("10", "90") };
            var koder = new List<Kode>
                            {
                                kode
                            };

            var melding = Builder<Lag>.CreateNew()
                .With(o => o.Koder = koder)
                .Build();

            var document = await repository.Opprett(melding);

            // Act
            var lagretLag = await repository.Hent(document.Id);

            // Assert
            lagretLag.Koder.FirstOrDefault().PosisjonTilstand.ShouldEqual(kode.PosisjonTilstand);
            lagretLag.Koder.FirstOrDefault().Bokstav.ShouldEqual(kode.Bokstav);
            lagretLag.Koder.FirstOrDefault().Gps.Latitude.ShouldEqual(kode.Gps.Latitude);
            lagretLag.Koder.FirstOrDefault().Gps.Longitude.ShouldEqual(kode.Gps.Longitude);
        }

        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Hent_LagMedLagId_LagHarLagId()
        {
            // Arrange
            var repository = OpprettRepository();

            const string LagId = "abc";

            var melding = Builder<Lag>.CreateNew()
                .With(o => o.LagId = LagId)
                .Build();

            var document = await repository.Opprett(melding);

            // Act
            var lagretLag = await repository.Hent(document.Id);

            // Assert
            lagretLag.LagId.ShouldEqual(LagId);
        }

        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Hent_LagMedMelding_LagHarMelding()
        {
            // Arrange
            var repository = OpprettRepository();

            var melding = new Melding {
                                  LagId = "abc",
                                  Tid = DateTime.Now,
                                  Tekst = "heiheiæøå",
                                  Type = MeldingType.Fritekst
                              };

            var meldinger = new List<Melding> { melding };
            
            var lag = Builder<Lag>.CreateNew()
                .With(o => o.Meldinger = meldinger)
                .Build();

            var document = await repository.Opprett(lag);

            // Act
            var lagretLag = await repository.Hent(document.Id);

            // Assert
            lagretLag.Meldinger.FirstOrDefault().LagId.ShouldEqual(melding.LagId);
            lagretLag.Meldinger.FirstOrDefault().Tekst.ShouldEqual(melding.Tekst);
            lagretLag.Meldinger.FirstOrDefault().Tid.ShouldEqual(melding.Tid);
            lagretLag.Meldinger.FirstOrDefault().Type.ShouldEqual(melding.Type);
        }

        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Hent_LagMedPifPosisjoner_LagHarPifPosisjon()
        {
            // Arrange
            var repository = OpprettRepository();
            
            var pifPosisjon = new PifPosisjon {
                                      LagId = "abc",
                                      Latitude = "12.12",
                                      Longitude = "10.123121",
                                      Tid = DateTime.Now
                                  };

            var pifPosisjoner = new List<PifPosisjon> { pifPosisjon };

            var lag = Builder<Lag>.CreateNew()
                .With(o => o.PifPosisjoner = pifPosisjoner)
                .Build();

            var document = await repository.Opprett(lag);

            // Act
            var lagretLag = await repository.Hent(document.Id);

            // Assert
            lagretLag.PifPosisjoner.FirstOrDefault().LagId.ShouldEqual(pifPosisjon.LagId);
            lagretLag.PifPosisjoner.FirstOrDefault().Latitude.ShouldEqual(pifPosisjon.Latitude);
            lagretLag.PifPosisjoner.FirstOrDefault().Longitude.ShouldEqual(pifPosisjon.Longitude);
            lagretLag.PifPosisjoner.FirstOrDefault().Tid.ShouldEqual(pifPosisjon.Tid);
        }

        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Oppdater_LagMedFlerePoeng_LagHarFlerePoeng()
        {
            // Arrange
            var repository = OpprettRepository();

            const int poeng = 10;
            const int poengØkning = 10;

            var lag = Builder<Lag>.CreateNew()
                .With(o => o.Poeng = poeng)
                .Build();

            var document = await repository.Opprett(lag);
            var opprettetLag = await repository.Hent(document.Id);

            // Act
            opprettetLag.Poeng += poengØkning;
            await repository.Oppdater(opprettetLag);

            // Assert
            var oppdatertLag = await repository.Hent(opprettetLag.DocumentId);

            oppdatertLag.Poeng.ShouldEqual(20);
        }

        private LagRepository OpprettRepository()
        {
            return new LagRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));
        }
    }
}