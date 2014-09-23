using System;
using System.Collections.Generic;
using BouvetCodeCamp.Felles.Entiteter;
using BouvetCodeCamp.Service.Interfaces;
using Moq;
using NUnit.Framework;

namespace BouvetCodeCamp.UnitTest
{
    [TestFixture]
    public class GameApiTest
    {
        private IGameApi _gameApi;
        private readonly Mock<IKodeService> _kodeService = new Mock<IKodeService>();
        private readonly Mock<ILagService> _lagService = new Mock<ILagService>();
        private readonly Mock<ILoggService> _loggService = new Mock<ILoggService>();
 
        [SetUp]
        public void Setup()
        {
            _gameApi = new GameApi(_kodeService.Object, _lagService.Object, this._loggService.Object);
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
            _lagService.Setup(x => x.HentLag(It.IsAny<string>()))
                .ReturnsAsync(lag);

            var resultat = await _gameApi.HentSistePifPositionForLag(String.Empty);

            Assert.AreEqual(tidligsteTid, resultat.Tid);
        }
    }
}
