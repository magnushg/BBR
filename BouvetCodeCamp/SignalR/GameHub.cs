using BouvetCodeCamp.OutputModels;
using Microsoft.AspNet.SignalR;
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
    }
    public class GameHub : Hub<IGameHub>
    {


        public void Send(PifPosisjonModel nyPifPosisjon)
        {
            Clients.All.NyPifPosisjon(nyPifPosisjon);

        }

    }
}
