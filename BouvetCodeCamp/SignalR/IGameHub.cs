namespace BouvetCodeCamp.SignalR
{
    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.Domene.OutputModels;

    public interface IGameHub
    {
        void NyPifPosisjon(PifPosisjonModell pifPosisjonModell);

        void SetRedZone(Koordinat koordinat);

        void NyLoggHendelse(LoggHendelseModell loggHendelseModell);
    }
}