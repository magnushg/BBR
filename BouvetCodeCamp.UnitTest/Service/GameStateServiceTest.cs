using System;
using System.Collections.Generic;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester.Interfaces;
using BouvetCodeCamp.DomeneTjenester.Services;
using Moq;
using NUnit.Framework;

namespace BouvetCodeCamp.UnitTest.Service
{
    using log4net;

    [TestFixture]
    public class GameStateServiceTest
    {
        private readonly Mock<IRepository<GameState>> _gameStateRepositoryMock = new Mock<IRepository<GameState>>();
        private readonly Mock<ILog> _logMock = new Mock<ILog>();
        private GameStateService _gameStateService;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Init_IngenIDatabase_NyBlirlagret()
        {
            _gameStateRepositoryMock.Setup(x => x.HentAlle()).Returns(new List<GameState>());
            _gameStateService = new GameStateService(_gameStateRepositoryMock.Object, new GameState());

            _gameStateRepositoryMock.Verify(x => x.Opprett(It.IsAny<GameState>()), Times.Once);
        }

        [Test]
        public void Init_DatabaseFins_PropertyIsSet()
        {
            var gameState = new GameState { DocumentId = Guid.NewGuid().ToString() };

            _gameStateRepositoryMock.Setup(x => x.HentAlle()).Returns(new List<GameState>
            {
                gameState
            });

            _gameStateService = new GameStateService(_gameStateRepositoryMock.Object, new GameState());

            Assert.AreEqual(gameState, _gameStateService.Hent(String.Empty));
        }

        [Test]
        [ExpectedException]
        [Ignore("rydder opp stille istedenfor, test ikke lenger gyldig")]
        public void Init_FlereGamestatesFins_Exception()
        {
            var gameState1 = new GameState { DocumentId = Guid.NewGuid().ToString() };
            var gameState2 = new GameState { DocumentId = Guid.NewGuid().ToString() };

            _gameStateRepositoryMock.Setup(x => x.HentAlle()).Returns(new List<GameState>
            {
                gameState1, gameState2
            });

            _gameStateService = new GameStateService(_gameStateRepositoryMock.Object, new GameState());
        }

        [Test]
        public void Hent_KallerIkkePåRepository()
        {
            var gameState1 = new GameState {DocumentId = Guid.NewGuid().ToString()};
            _gameStateRepositoryMock.Setup(x => x.HentAlle()).Returns(new List<GameState> {gameState1});
            _gameStateService = new GameStateService(_gameStateRepositoryMock.Object, new GameState());

            _gameStateService.Hent(It.IsAny<String>());

            _gameStateRepositoryMock.Verify(x => x.Hent(It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void Oppdater_KallerPaReposotory()
        {
            var gameState1 = new GameState { DocumentId = Guid.NewGuid().ToString() };
            _gameStateRepositoryMock.Setup(x => x.HentAlle()).Returns(new List<GameState> { gameState1 });

            _gameStateService = new GameStateService(_gameStateRepositoryMock.Object, new GameState());
            _gameStateService.Oppdater(It.IsAny<GameState>());

            _gameStateRepositoryMock.Verify(x => x.Oppdater(It.IsAny<GameState>()), Times.Once);
        }

        [Test]
        public void Oppdater_SetterPropertyTilNyInstans()
        {
            var gameState = new GameState { DocumentId = Guid.NewGuid().ToString() };

            _gameStateService.Oppdater(gameState);

            Assert.AreSame(gameState, _gameStateService.Hent(String.Empty));
        }

    }
}
