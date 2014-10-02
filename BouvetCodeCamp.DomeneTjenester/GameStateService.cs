using System;
using System.Linq;
using System.Threading.Tasks;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester.Interfaces;

namespace BouvetCodeCamp.DomeneTjenester
{
    /// <summary>
    /// Tanken med denne servicen er for å abstrahere bort gamestaterepository
    /// siden det kun skal eksistere 1 gamestate om gangen.
    /// </summary>
    public class GameStateService : IGameStateService
    {
        private readonly IRepository<GameState> _gameStateRepository;

        public GameStateService(IRepository<GameState> gameStateRepository)
        {
            _gameStateRepository = gameStateRepository;
        }

        public GameState HentGameState()
        {
            var gameStates = _gameStateRepository.HentAlle().ToList();

            switch (gameStates.Count())
            {
                case 0:
                    var gameState = new GameState();
                    _gameStateRepository.Opprett(gameState);
                    return gameState;
                case 1:
                    return gameStates.Single();
                default:
                    throw new Exception("Multiple gamestates found");
            }
        }

        public async Task OppdaterGameState(GameState gameState)
        {
            await _gameStateRepository.Oppdater(gameState);
        }
    }
}
