using BouvetCodeCamp.Domene.Entiteter;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    public interface IKoordinatVerifier
    {
        bool KoordinaterErNærHverandre(Koordinat first, Koordinat second);
        bool KoordinatErInnenforPolygonet(Koordinat koordinat, Koordinat[] polygon);
    }
}