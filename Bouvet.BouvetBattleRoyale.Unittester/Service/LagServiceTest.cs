namespace Bouvet.BouvetBattleRoyale.Unittests.Service
{
    using System;
    using System.Collections.Generic;

    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Tjenester.Interfaces;
    using Bouvet.BouvetBattleRoyale.Tjenester.Services;

    using BouvetCodeCamp.Domene.Entiteter;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    class LagGameServiceTest
    {
        private LagGameService _lagGameService;
        private readonly Mock<IRepository<Lag>> _lagRepository = new Mock<IRepository<Lag>>();

        [SetUp]
        public void Setup()
        {
            _lagGameService = new LagGameService(_lagRepository.Object);
        }

        [Test]
        public void HentSistePifPositionForLag_ReturnererNyligstePif()
        {
            var lag = new Lag();
            var tidligsteTid = new DateTime(2000, 1, 1);
            var senesteTid = new DateTime(2009, 1, 1);

            lag.PifPosisjoner = new List<PifPosisjon>
            {
                new PifPosisjon {Tid = new DateTime(2001, 1, 1)},
                new PifPosisjon {Tid = tidligsteTid},
                new PifPosisjon {Tid = senesteTid},
                new PifPosisjon {Tid = new DateTime(2003, 1, 1)},
            };

            _lagRepository.Setup(x => x.Søk(It.IsAny<Func<Lag, bool>>())).Returns(new []
            {
                lag
            });

            var sistePifPosisjon = _lagGameService.HentSistePifPosisjon(String.Empty);

            Assert.AreEqual(senesteTid, sistePifPosisjon.Tid);
        }
    }
}
