using System.Collections.Generic;
using System.Net;
using BouvetCodeCamp.Domene.InputModels;
using BouvetCodeCamp.Domene.OutputModels;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    using System.Threading.Tasks;

    public interface IGameApi
    {
        Task RegistrerPifPosisjon(Domene.InputModels.PifPosisjonInputModell inputModell);

        PifPosisjonOutputModell HentSistePifPositionForLag(string lagId);
        
        Task<bool> RegistrerKode(PostInputModell inputModell);

        Task SendMelding(MeldingInputModell inputModell);

        IEnumerable<KodeOutputModel> HentRegistrerteKoder(string lagId);

        PostOutputModell HentGjeldendePost(string lagId);

        Task TildelPoeng(PoengInputModell inputModell);
    }
}
