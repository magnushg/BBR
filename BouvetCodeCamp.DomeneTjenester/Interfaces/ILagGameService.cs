using BouvetCodeCamp.Domene.Entiteter;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    public interface ILagGameService
    {
        Lag HentLagMedLagId(string lagId);

        PifPosisjon HentSistePifPosisjon(string lagId);
    }
}