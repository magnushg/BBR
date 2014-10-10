namespace BouvetCodeCamp.SignalR
{
    using Domene.OutputModels;

    public interface IGameHub
    {
        void NyPifPosisjon(PifPosisjonOutputModell pifPosisjonOutputModell);

        void SettInfisertSone(InfisertPolygonOutputModell infisertPolygonOutputModell);

        void NyLoggHendelse(LoggHendelseOutputModell loggHendelseOutputModell);
    }
}