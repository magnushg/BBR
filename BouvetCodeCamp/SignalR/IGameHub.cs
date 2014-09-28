namespace BouvetCodeCamp.SignalR
{
    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.Domene.OutputModels;

    public interface IGameHub
    {
        void NyPifPosisjon(PifPosisjonModel nyPifPosisjon);
        void SetRedZone(Koordinat koordinat);
    }
}