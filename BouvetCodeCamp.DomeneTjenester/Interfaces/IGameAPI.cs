using System.Collections.Generic;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.Domene.InputModels;
using BouvetCodeCamp.Domene.OutputModels;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    using System.Threading.Tasks;

    public interface IGameApi
    {
        Task RegistrerPifPosisjon(PifPosisjonInputModell inputModell);

        PifPosisjonOutputModell HentSistePifPositionForLag(string lagId);

        Task<bool> RegistrerKode(KodeInputModell inputModell);

        Task SendMelding(MeldingInputModell inputModell);

        IEnumerable<KodeOutputModel> HentRegistrerteKoder(string lagId);

        PostOutputModell HentGjeldendePost(string lagId);

        Task TildelPoeng(PoengInputModell inputModell);

        bool ErLagPifInnenInfeksjonssone(string lagId);
    }
}
