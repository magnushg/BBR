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
        private readonly Mock<IPostService> _kodeService = new Mock<IPostService>();
        private readonly Mock<ILagService> _lagService = new Mock<ILagService>();
        private readonly Mock<IGameStateService> _gameStateService = new Mock<IGameStateService>();
        private readonly Mock<IKoordinatVerifier> _koordinatVerifier = new Mock<IKoordinatVerifier>();
        private readonly Mock<IPoengService> _poengServiceMock = new Mock<IPoengService>();

        [SetUp]
        public void Setup()
        {
            _gameApi = new GameApi(_kodeService.Object,
                _lagService.Object,
                _koordinatVerifier.Object,
                _gameStateService.Object,
                _poengServiceMock.Object
                );
        }

        [Test]
        public void ErLagPifInnenInfeksjonssone_ErInnenInfeksjonssone_ReturnsTrue()
        {
            var gamestate = new GameState { InfisertPolygon = new InfisertPolygon()};
            var pif = new PifPosisjon();

            _lagService.Setup(x => x.HentSistePifPosisjon(It.IsAny<string>())).Returns(() => pif);
            _gameStateService.Setup(x => x.HentGameState()).Returns(() => gamestate);
            _koordinatVerifier.Setup(
                x => x.KoordinatErInnenforPolygonet(pif.Posisjon, gamestate.InfisertPolygon.Koordinater)).Returns(true);

            var result = _gameApi.ErLagPifInnenInfeksjonssone(String.Empty);

            Assert.IsTrue(result);
        }
    }
}
