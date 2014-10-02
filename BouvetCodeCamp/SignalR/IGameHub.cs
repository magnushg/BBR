namespace BouvetCodeCamp.SignalR
{
    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.Domene.OutputModels;

    public interface IGameHub
    {
        void NyPifPosisjon(PifPosisjonOutputModell pifPosisjonOutputModell);

        void SetRedZone(Koordinat koordinat);

        void NyLoggHendelse(LoggHendelseOutputModell loggHendelseOutputModell);
    }
}