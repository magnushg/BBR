using BouvetCodeCamp.Domene.Entiteter;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    public interface IGameStateService
    {
        GameState HentGameState();
        void OppdaterGameState(GameState gameState);

    }
}
