using System;
using System.Collections.Generic;
using BouvetCodeCamp.Domene;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester;
using BouvetCodeCamp.DomeneTjenester.Interfaces;
using NUnit.Framework;
using Should;

namespace BouvetCodeCamp.UnitTest.Service
{
    using BouvetCodeCamp.SignalR.Hubs;

    using Microsoft.AspNet.SignalR;

    using Moq;

    [TestFixture]
    class PoengServiceTest
    {
        private IPoengService _poengService;
        private PoengTildeling _poengTildeling;
        private readonly Mock<IKoordinatVerifier> _coordinatMock = new Mock<IKoordinatVerifier>();
        private readonly Mock<Lazy<IHubContext<IGameHub>>> _gameHubMock = new Mock<Lazy<IHubContext<IGameHub>>>();

        [SetUp]
        public void Startup()
        {
            _poengTildeling = new PoengTildeling();

            _poengService = new PoengService(_poengTildeling, _gameHubMock.Object);

            _poengTildeling.InfisertTickStraff = 1;
            _poengTildeling.InfisertTidssfrist = 1;
            _poengTildeling.KodeOppdaget = 1;
            _poengTildeling.MeldingsStraff = 1;
            _poengTildeling.PingTimeoutStraff = 1;
            _poengTildeling.PingTimeout = 1;
        }

        [Test]
        public void SjekkOgSettPifPingStraff_Timeout1Sekund_1PingStraff()
        {
            _poengTildeling.PingTimeout = 1;
            _poengTildeling.PingTimeoutStraff = -1;
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
            _poengTildeling.PingTimeout = 1;
            _poengTildeling.PingTimeoutStraff = -10;

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
            _poengTildeling.PingTimeout = 1;
            _poengTildeling.PingTimeoutStraff = -1;

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
            _poengTildeling.PingTimeout = 10;

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
            _poengTildeling.PingTimeout = 10;

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
            _poengTildeling.InfisertTickStraff = 10;
            _poengTildeling.InfisertTidssfrist = 5;

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
            _poengTildeling.InfisertTidssfrist = 5;
            _poengTildeling.InfisertTickStraff = -1;

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
            _poengTildeling.InfisertTickStraff = -1;
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
            _poengTildeling.InfisertTickStraff = -10;
            _poengTildeling.InfisertTidssfrist = 5;

            var startPoeng = 10;
            var lag = new Lag
            {
                PifPosisjoner = new List<PifPosisjon>
                {
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 1), Infisert = true },
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 1 + _poengTildeling.InfisertTidssfrist + 1), Infisert = true },
                },
                Poeng = startPoeng
            };

            lag = _poengService.SjekkOgSettInfisertSoneStraff(lag);

            lag.Poeng.ShouldEqual(startPoeng + (int)_poengTildeling.InfisertTickStraff);
        }

        [Test]
        [Ignore]
        public void SjekkOgSettInfisertSoneStraff_TidUtgattMed3Sekund_3TickStraff()
        {
            _poengTildeling.InfisertTickStraff = 10;
            _poengTildeling.InfisertTidssfrist = 5;

            var startPoeng = 10;
            var lag = new Lag
            {
                PifPosisjoner = new List<PifPosisjon>
                {
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 6), Infisert = true },
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 5), Infisert = false },
                    new PifPosisjon { Tid = new DateTime(2000, 1, 1, 1, 1, 6 + _poengTildeling.InfisertTidssfrist + 3), Infisert = true },
                },
                Poeng = 10
            };

            lag = _poengService.SjekkOgSettInfisertSoneStraff(lag);

            lag.Poeng.ShouldEqual(startPoeng - (int)_poengTildeling.InfisertTickStraff * 3);
        }

    }
}
