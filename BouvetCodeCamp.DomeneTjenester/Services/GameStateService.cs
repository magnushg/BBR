using System.Threading.Tasks;

namespace BouvetCodeCamp.DomeneTjenester.Services
{
    using System.Linq;

    using Domene.Entiteter;
    using Interfaces;

    using log4net;

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
            GameState gameState
            )
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
                    //noe har skjedd, cleanup og start over
                
                    gameStates.ForEach(x => _gameStateRepository.Slett(x));
                    _gameStateRepository.Opprett(_gameState);
                    break;
            }
        }

        public override GameState Hent(string id)
        {
            return _gameState;
        }

        public override Task Oppdater(GameState entitet)
        {
            //dersom det fins flere (som det ikke skal gjøre, men det hender av en eller annen grunn)
            //så fjern de andre
            var gameStates = _gameStateRepository.HentAlle().ToList();
            if (gameStates.Count > 1)
            {
         
                foreach (var gameState in gameStates)
                {
                    if (gameState.DocumentId != entitet.DocumentId)
                        _gameStateRepository.Slett(gameState);
                }
            }

            _gameState = entitet;

            return _gameStateRepository.Oppdater(entitet);
        }
    }
}