namespace Bouvet.BouvetBattleRoyale.Tjenester.SignalR.Hubs
{
    using Bouvet.BouvetBattleRoyale.Domene.OutputModels;
    using Bouvet.BouvetBattleRoyale.Tjenester.Interfaces;

    using Microsoft.AspNet.SignalR;

    public class GameHubProxy : IGameHub
    {
        private readonly IHubContext<IGameHub> _hubContext;

        public GameHubProxy(IHubContext<IGameHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void NyPifPosisjon(PifPosisjonOutputModell pifPosisjonOutputModell)
        {
            _hubContext.Clients.All.NyPifPosisjon(pifPosisjonOutputModell);
        }

        public void SettInfisertSone(InfisertPolygonOutputModell infisertPolygonOutputModell)
        {
            _hubContext.Clients.All.SettInfisertSone(infisertPolygonOutputModell);
        }

        public void NyLoggHendelse(LoggHendelseOutputModell loggHendelseOutputModell)
        {
            _hubContext.Clients.All.NyLoggHendelse(loggHendelseOutputModell);
        }

        public void PoengTildelt(PoengOutputModell poengOutputModell)
        {
            _hubContext.Clients.All.PoengTildelt(poengOutputModell);
        }

        public void NyPostRegistrert(PostRegistrertOutputModell postRegistrertOutputModell)
        {
            _hubContext.Clients.All.NyPostRegistrert(postRegistrertOutputModell);
        }
    }
}