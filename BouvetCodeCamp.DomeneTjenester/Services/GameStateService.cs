namespace BouvetCodeCamp.DomeneTjenester.Services
{
    using System;
    using System.Linq;

    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;

    /// <summary>
    /// Tanken med denne servicen er for å abstrahere bort gamestaterepository
    /// siden det kun skal eksistere 1 gamestate om gangen.
    /// </summary>
    public class GameStateService : Service<GameState>
    {
        private readonly IRepository<GameState> _gameStateRepository;

        public GameStateService(IRepository<GameState> gameStateRepository) : base(gameStateRepository)
        {
            _gameStateRepository = gameStateRepository;
        }

        public override GameState Hent(string id)
        {
            // Henter kun en gamestate, ignorerer id på gamestateobjektet
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
    }
}