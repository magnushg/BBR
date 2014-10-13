using System.Threading.Tasks;

namespace BouvetCodeCamp.DomeneTjenester.Services
{
    using System;
    using System.Linq;

    using Domene.Entiteter;
    using Interfaces;

    /// <summary>
    /// Tanken med denne servicen er for å abstrahere bort gamestaterepository
    /// siden det kun skal eksistere 1 gamestate om gangen.
    /// </summary>
    public class GameStateService : Service<GameState>
    {
        private readonly IRepository<GameState> _gameStateRepository;
        private static GameState _gameState;

        public GameStateService(
            IRepository<GameState> gameStateRepository,
            GameState gameState)
            : base(gameStateRepository)
        {
            _gameStateRepository = gameStateRepository;
            _gameState = gameState;

            var gameStates = _gameStateRepository.HentAlle().ToList();

            switch (gameStates.Count())
            {
                case 0:
                    //ingen gamestate i db, sett til tom instans.
                    _gameStateRepository.Opprett(_gameState);
                    break;
                case 1:
                    _gameState = gameStates.Single();
                    break;
                default:
                    throw new Exception("Multiple gamestates found in db");
            }
        }

        public override GameState Hent(string id)
        {
            return _gameState;
        }

        public override Task Oppdater(GameState entitet)
        {
            _gameState = entitet;
            return _gameStateRepository.Oppdater(entitet);
        }
    }
}