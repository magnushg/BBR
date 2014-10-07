using System.Collections.Generic;
using System.Net;
using BouvetCodeCamp.Domene.InputModels;
using BouvetCodeCamp.Domene.OutputModels;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    using System.Threading.Tasks;

    using BouvetCodeCamp.Domene.Entiteter;

    public interface IGameApi
    {
        Task RegistrerPifPosisjon(PifPosisjonInputModell inputModell);

        PifPosisjonOutputModell HentSistePifPositionForLag(string lagId);
        
        Task<bool> RegistrerKode(PostInputModell inputModell);

        Task SendMelding(MeldingInputModell inputModell);

        IEnumerable<KodeOutputModel> HentRegistrerteKoder(string lagId);

        PostOutputModell HentGjeldendePost(string lagId);

        Task TildelPoeng(PoengInputModell inputModell);

        bool ErLagPifInnenInfeksjonssone(string lagId);

        IEnumerable<Melding> HentMeldinger(string lagId);
    }
}
