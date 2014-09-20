using BouvetCodeCamp.Felles.Entiteter;
using BouvetCodeCamp.OutputModels;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
