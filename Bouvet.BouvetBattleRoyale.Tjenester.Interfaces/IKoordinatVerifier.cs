namespace Bouvet.BouvetBattleRoyale.Tjenester.Interfaces
{
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;

    public interface IKoordinatVerifier
    {
        bool KoordinaterErNærHverandre(Koordinat first, Koordinat second);
        bool KoordinatErInnenforPolygonet(Koordinat koordinat, Koordinat[] polygon);
    }
}