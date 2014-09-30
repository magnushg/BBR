namespace BouvetCodeCamp.SignalR
{
    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.Domene.OutputModels;

    public interface IGameHub
    {
        void NyPifPosisjon(PifPosisjonModell nyPifPosisjon);
        void SetRedZone(Koordinat koordinat);
    }
}