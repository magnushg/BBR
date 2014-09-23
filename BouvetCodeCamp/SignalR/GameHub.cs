using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.Domene.OutputModels;
using Microsoft.AspNet.SignalR;

namespace BouvetCodeCamp.SignalR
{
    public interface IGameHub
    {
        void NyPifPosisjon(PifPosisjonModel nyPifPosisjon);
        void SetRedZone(Coordinate coordinate);
    }
    
    public class GameHub : Hub<IGameHub>
    {

    }
}
