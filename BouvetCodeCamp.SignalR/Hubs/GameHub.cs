using System;
using BouvetCodeCamp.Domene.OutputModels;

namespace BouvetCodeCamp.SignalR.Hubs
{
    using Microsoft.AspNet.SignalR;

    public class GameHub : Hub<IGameHub>
    {
    }

    public class GameHubProxy : IGameHub
    {
        private IHubContext<IGameHub> _hubContext;
        public GameHubProxy(IHubContext<IGameHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public void NyPifPosisjon(Domene.OutputModels.PifPosisjonOutputModell pifPosisjonOutputModell)
        {
            _hubContext.Clients.All.NyPifPosisjon(pifPosisjonOutputModell);
        }

        public void SettInfisertSone(Domene.OutputModels.InfisertPolygonOutputModell infisertPolygonOutputModell)
        {
            _hubContext.Clients.All.SettInfisertSone(infisertPolygonOutputModell);
        }

        public void NyLoggHendelse(Domene.OutputModels.LoggHendelseOutputModell loggHendelseOutputModell)
        {
            _hubContext.Clients.All.NyLoggHendelse(loggHendelseOutputModell);
        }

        public void PoengTildelt(Domene.OutputModels.PoengOutputModell poengOutputModell)
        {
            _hubContext.Clients.All.PoengTildelt(poengOutputModell);
        }

        public void NyPostRegistrert(Domene.OutputModels.PostRegistrertOutputModell postRegistrertOutputModell)
        {
            _hubContext.Clients.All.NyPostRegistrert(postRegistrertOutputModell);
        }
    }
}

               
