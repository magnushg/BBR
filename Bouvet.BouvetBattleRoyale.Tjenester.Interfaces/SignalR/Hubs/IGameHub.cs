namespace Bouvet.BouvetBattleRoyale.Tjenester.Interfaces.SignalR.Hubs
{
    using Bouvet.BouvetBattleRoyale.Domene.OutputModels;

    public interface IGameHub
    {
        void NyPifPosisjon(PifPosisjonOutputModell pifPosisjonOutputModell);

        void SettInfisertSone(InfisertPolygonOutputModell infisertPolygonOutputModell);

        void NyLoggHendelse(LoggHendelseOutputModell loggHendelseOutputModell);

        void PoengTildelt(PoengOutputModell poengOutputModell);

        void NyPostRegistrert(PostRegistrertOutputModell postRegistrertOutputModell);
    }
}