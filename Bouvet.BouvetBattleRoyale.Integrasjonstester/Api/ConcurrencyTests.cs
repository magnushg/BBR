﻿namespace Bouvet.BouvetBattleRoyale.Integrasjonstester.Api
{
    using System;
    using System.Threading;

    using Bouvet.BouvetBattleRoyale.Domene;
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;

    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;
    using BouvetCodeCamp.Integrasjonstester.Api;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ConcurrencyTests : BaseApiTest
    {
        [TestInitialize]
        [TestCleanup]
        public void RyddOppEtterTest()
        {
            SlettLag(TestLagId);
        }

        [TestMethod]
        public void VerifiserOptimisticConcurrency()
        {
            SørgForAtEtLagFinnes();
            var lagGameService = Resolve<ILagGameService>();
            var lagService = Resolve<IService<Lag>>();

            // Hent ut to instanser av lag
            var lagMedMelding = lagGameService.HentLagMedLagId(TestLagId);
            var lagMedPosisjon = lagGameService.HentLagMedLagId(TestLagId);
            var pifPosisjon = new PifPosisjon
            {
                Posisjon = new Koordinat { Latitude = "59.10", Longitude = "10.6" },
                LagId = TestLagId,
                Tid = DateTime.Now,
                Infisert = false
            };
            lagMedPosisjon.PifPosisjoner.Add(pifPosisjon);
            var melding = new Melding
            {
                LagId = TestLagId,
                Tekst = "1",
                Tid = DateTime.Now,
                Type = MeldingType.Lengde
            };
            lagMedMelding.Meldinger.Add(melding);

            // Lagre melding
            lagService.Oppdater(lagMedMelding);

            // Vent godt for å sikre seg at alt er på plass
            Thread.Sleep(5000);

            var lagEtterMeldingsLagring = lagGameService.HentLagMedLagId(TestLagId);
            Assert.AreEqual(1, lagEtterMeldingsLagring.Meldinger.Count, "Meldingen skulle blitt lagret.");
            
            // Lagrer så posisjonen
            lagService.Oppdater(lagMedPosisjon);

            Thread.Sleep(5000);

            var lagEtterPosisjonsLagring = lagGameService.HentLagMedLagId(TestLagId);

            Assert.AreEqual(1, lagEtterPosisjonsLagring.PifPosisjoner.Count, "Posisjonen skulle blitt lagret.");

            //MEN:
            Assert.AreEqual(1, lagEtterPosisjonsLagring.Meldinger.Count, "WHAAAT? Meldingen ble slettet :(");
        }
    }
}