using System;
using System.Collections.Generic;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester.Interfaces;
using Moq;
using NUnit.Framework;

namespace BouvetCodeCamp.UnitTest
{
    using GameApi = BouvetCodeCamp.DomeneTjenester.GameApi;

    [TestFixture]
    public class GameApiTest
    {
        private IGameApi _gameApi;
        private readonly Mock<IPostService> _kodeService = new Mock<IPostService>();
        private readonly Mock<ILagService> _lagService = new Mock<ILagService>();
 
        [SetUp]
        public void Setup()
        {
            _gameApi = new GameApi(_kodeService.Object, _lagService.Object);
        }

        [Test]
        public async void HentSistePifPositionForLag_ReturnererNyligstePif()
        {
            var lag = new Lag();
            var tidligsteTid = new DateTime(2000, 1, 1);

            lag.PifPosisjoner = new List<PifPosisjon>
            {
                new PifPosisjon {Tid = new DateTime(2001, 1, 1)},
                new PifPosisjon {Tid = tidligsteTid},
                new PifPosisjon {Tid = new DateTime(2002, 1, 1)},
                new PifPosisjon {Tid = new DateTime(2003, 1, 1)},
            };

            _lagService.Setup(x => x.HentLagMedLagId(It.IsAny<string>())).Returns(lag);

            var resultat = _gameApi.HentSistePifPositionForLag(String.Empty);

            Assert.AreEqual(tidligsteTid, resultat.Tid);
        }
    }
}
