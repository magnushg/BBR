using System.Threading.Tasks;
using BouvetCodeCamp.Domene.Entiteter;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    public interface IGameStateService
    {
        GameState HentGameState();
        Task OppdaterGameState(GameState gameState);

    }
}
