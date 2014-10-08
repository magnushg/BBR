using System;
using System.Collections.Generic;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester.Interfaces;
using Moq;
using NUnit.Framework;

namespace BouvetCodeCamp.UnitTest
{
    using GameApi = DomeneTjenester.GameApi;

    [TestFixture]
    public class GameApiTest
    {
        private IGameApi _gameApi;
        private readonly Mock<IPostGameService> _postGameService = new Mock<IPostGameService>();
        private readonly Mock<ILagGameService> _lagGameService = new Mock<ILagGameService>();
        private readonly Mock<IService<Lag>> _lagService = new Mock<IService<Lag>>();
        private readonly Mock<IService<GameState>> _gameStateService = new Mock<IService<GameState>>();
        private readonly Mock<IKoordinatVerifier> _koordinatVerifier = new Mock<IKoordinatVerifier>();

        [SetUp]
        public void Setup()
        {
            _gameApi = new GameApi(
                _postGameService.Object,
                _lagGameService.Object,
                _lagService.Object,
                _koordinatVerifier.Object,
                _gameStateService.Object);
        }

        [Test]
        public void ErLagPifInnenInfeksjonssone_ErInnenInfeksjonssone_ReturnsTrue()
        {
            var gamestate = new GameState { InfisertPolygon = new InfisertPolygon()};
            var pif = new PifPosisjon();

            _lagGameService.Setup(x => x.HentSistePifPosisjon(It.IsAny<string>())).Returns(() => pif);
            _gameStateService.Setup(x => x.Hent(string.Empty)).Returns(() => gamestate);
            _koordinatVerifier.Setup(
                x => x.KoordinatErInnenforPolygonet(pif.Posisjon, gamestate.InfisertPolygon.Koordinater)).Returns(true);

            var result = _gameApi.ErLagPifInnenInfeksjonssone(String.Empty);

            Assert.IsTrue(result);
        }
    }
}
