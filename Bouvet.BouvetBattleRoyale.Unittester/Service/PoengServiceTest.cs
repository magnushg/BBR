namespace Bouvet.BouvetBattleRoyale.Unittests.Service
{
    using System;
    using System.Collections.Generic;

    using Bouvet.BouvetBattleRoyale.Domene;
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Tjenester.SignalR.Hubs;

    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.DomeneTjenester;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;

    using Moq;

    using NUnit.Framework;

    using Should;

    [TestFixture]
    class PoengServiceTest
    {
        private IPoengService _poengService;
        private readonly Mock<IKoordinatVerifier> _coordinatMock = new Mock<IKoordinatVerifier>();
        private readonly Mock<IGameHub> _gameHubMock = new Mock<IGameHub>();
        
        [SetUp]
        public void Startup()
        {
            _poengService = new PoengService(_gameHubMock.Object);

            PoengTildeling.InfisertTickStraff = 1;
            PoengTildeling.InfisertTidssfrist = 1;
            PoengTildeling.KodeOppdaget = 1;
            PoengTildeling.MeldingsStraff = 1;
            PoengTildeling.PingTimeoutStraff = 1;
            PoengTildeling.PingTimeout = 1;
        }

        [Test]
        public void SettFritekstMeldingSendtStraff_ikkeFritekst_IngenStraff()
        {
            PoengTildeling.MeldingsStraff = -2;
            const int startPoeng = 10;
            var melding = new Melding
            {
                Type = MeldingType.Himmelretning
            };

            var lag = new Lag {Poeng = startPoeng};
            lag = _poengService.SettFritekstMeldingSendtStraff(lag, melding);

            Assert.AreEqual(startPoeng, lag.Poeng);
        }

        [Test]
        public void SettFritekstMeldingSendtStraff_fritekst_FarStraff()
        {
            PoengTildeling.MeldingsStraff = -2;
            const int startPoeng = 10;
            var melding = new Melding
            {
                Type = MeldingType.Fritekst,
                Tekst = "123 5"
            };
            var length = melding.Tekst.Length;

            var lag = new Lag {Poeng = startPoeng};
            lag = _poengService.SettFritekstMeldingSendtStraff(lag, melding);

            var forventetResultat = 0;
            Assert.AreEqual(0, lag.Poeng);
        }

        [Test]
        public void SjekkOgSettPifPingStraff_Timeout1Sekund_1PingStraff()
        {
            PoengTildeling.PingTimeout = 1;
            PoengTildeling.PingTimeoutStraff = -1;
            const int startPoeng = 10;

            var lag = new Lag
            {
                PifPosisjoner = new List<PifPosisjon>
                {
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 1)},
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 3)}
                },
                Poeng = startPoeng
            };
            lag = _poengService.SjekkOgSettPifPingStraff(lag);

            lag.Poeng.ShouldEqual(9);
        }

        [Test]
        public void SjekkOgSettPifPingStraff_TimeoutMed5Sekund_1PingStraff()
        {
            PoengTildeling.PingTimeout = 1;
            PoengTildeling.PingTimeoutStraff = -10;

            const int startPoeng = 10;
            var lag = new Lag
            {
                PifPosisjoner = new List<PifPosisjon>
                {
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 1)},
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 2)},
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 2 + 5)}
                },
                Poeng = startPoeng
            };
            lag = _poengService.SjekkOgSettPifPingStraff(lag);

            lag.Poeng.ShouldEqual(0);
        }

        [Test]
        public void SjekkOgSettPifPingStraff_2TimeOuts_2PingStraff()
        {
            PoengTildeling.PingTimeout = 1;
            PoengTildeling.PingTimeoutStraff = -1;

            const int startPoeng = 10;
            var lag = new Lag
            {
                PifPosisjoner = new List<PifPosisjon>
                {
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 8)},
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 2)},
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 1)},
                },
                Poeng = startPoeng
            };

            lag = _poengService.SjekkOgSettPifPingStraff(lag);

            lag.Poeng.ShouldEqual(9);

            lag.PifPosisjoner.Add(new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 10)});
            lag = _poengService.SjekkOgSettPifPingStraff(lag);

            lag.Poeng.ShouldEqual(8);
        }


        [Test]
        public void SjekkOgSettPifPingStraff_NoTimeout_IngenStraff()
        {
            PoengTildeling.PingTimeout = 10;

            var startPoeng = 10;
            var lag = new Lag
            {
                PifPosisjoner = new List<PifPosisjon>
                {
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 1)},
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 11)}
                },
                Poeng = 10
            };
            lag = _poengService.SjekkOgSettPifPingStraff(lag);

            lag.Poeng.ShouldEqual(startPoeng);
        }

        [Test]
        public void SjekkOgSettPifPingStraff_NoTimeout_IngenStraff2()
        {
            PoengTildeling.PingTimeout = 10;

            var startPoeng = 10;
            var lag = new Lag
            {
                PifPosisjoner = new List<PifPosisjon>
                {
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 1)},
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 2)}
                },
                Poeng = 10
            };
            lag = _poengService.SjekkOgSettPifPingStraff(lag);

            lag.Poeng.ShouldEqual(startPoeng);
        }

        [Test]
        [Ignore]
        public void SjekkOgSettInfisertSoneStraff_TidIkkeUtgatt_IngenStraff1()
        {
            PoengTildeling.InfisertTickStraff = 10;
            PoengTildeling.InfisertTidssfrist = 5;

            var startPoeng = 10;
            var lag = new Lag
            {
                PifPosisjoner = new List<PifPosisjon>
                {
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 1), Infisert = true },
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 2), Infisert = true },
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 3), Infisert = true },
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 4), Infisert = true },
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 5), Infisert = true },
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 6), Infisert = true }
                },
                Poeng = 10
            };

            lag = _poengService.SjekkOgSettInfisertSoneStraff(lag);

            lag.Poeng.ShouldEqual(startPoeng);
        }

        [Test]
        [Ignore]
        public void SjekkOgSettInfisertSoneStraff_TidIkkeUtgatt_IngenStraff2()
        {
            PoengTildeling.InfisertTidssfrist = 5;
            PoengTildeling.InfisertTickStraff = -1;

            var startPoeng = 10;
            var lag = new Lag
            {
                PifPosisjoner = new List<PifPosisjon>
                {
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 2), Infisert = true },
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 1), Infisert = false },
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 3), Infisert = true },
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 5), Infisert = true },
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 4), Infisert = true },
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 6), Infisert = true }
                },
                Poeng = 10
            };

            lag = _poengService.SjekkOgSettInfisertSoneStraff(lag);

            lag.Poeng.ShouldEqual(startPoeng);
        }

        [Test]
        public void SjekkOgSettInfisertSoneStraff_InfisertI3Sekund_3TickStraff()
        {
            PoengTildeling.InfisertTickStraff = -1;
            var tid = 3;
            var startPoeng = 10;
            var lag = new Lag
            {
                PifPosisjoner = new List<PifPosisjon>
                {
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 1), Infisert = true },
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 2), Infisert = false },
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 3), Infisert = true },
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 3 + tid), Infisert = true },
                },
                Poeng = startPoeng
            };

            lag = _poengService.SjekkOgSettInfisertSoneStraff(lag);

            Assert.AreEqual(7, lag.Poeng);
        }

        [Test]
        [Ignore]
        public void SjekkOgSettInfisertSoneStraff_TidUtgattMed1Sekund_1TickStraff()
        {
            PoengTildeling.InfisertTickStraff = -10;
            PoengTildeling.InfisertTidssfrist = 5;

            var startPoeng = 10;
            var lag = new Lag
            {
                PifPosisjoner = new List<PifPosisjon>
                {
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 1), Infisert = true },
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 1 + PoengTildeling.InfisertTidssfrist + 1), Infisert = true },
                },
                Poeng = startPoeng
            };

            lag = _poengService.SjekkOgSettInfisertSoneStraff(lag);

            lag.Poeng.ShouldEqual(startPoeng + (int)PoengTildeling.InfisertTickStraff);
        }

        [Test]
        [Ignore]
        public void SjekkOgSettInfisertSoneStraff_TidUtgattMed3Sekund_3TickStraff()
        {
            PoengTildeling.InfisertTickStraff = 10;
            PoengTildeling.InfisertTidssfrist = 5;

            var startPoeng = 10;
            var lag = new Lag
            {
                PifPosisjoner = new List<PifPosisjon>
                {
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 6), Infisert = true },
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 5), Infisert = false },
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 6 + PoengTildeling.InfisertTidssfrist + 3), Infisert = true },
                },
                Poeng = 10
            };

            lag = _poengService.SjekkOgSettInfisertSoneStraff(lag);

            lag.Poeng.ShouldEqual(startPoeng - (int)PoengTildeling.InfisertTickStraff * 3);
        }

    }
}
