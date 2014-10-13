namespace BouvetCodeCamp.SignalR.Hubs
{
    using BouvetCodeCamp.Domene.InputModels;

    using Domene.OutputModels;

    public interface IGameHub
    {
        void NyPifPosisjon(PifPosisjonOutputModell pifPosisjonOutputModell);

        void SettInfisertSone(InfisertPolygonOutputModell infisertPolygonOutputModell);

        void NyLoggHendelse(LoggHendelseOutputModell loggHendelseOutputModell);

        void PoengTildelt(PoengOutputModell poengOutputModell);
    }
}