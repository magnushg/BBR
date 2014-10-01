using System.Collections.Generic;

using BouvetCodeCamp.Domene.InputModels;
using BouvetCodeCamp.Domene.OutputModels;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    using System.Threading.Tasks;

    using PifPosisjonModell = BouvetCodeCamp.Domene.OutputModels.PifPosisjonModell;

    public interface IGameApi
    {
        Task RegistrerPifPosisjon(Domene.InputModels.PifPosisjonModell modell);

        PifPosisjonModell HentSistePifPositionForLag(string lagId);
        
        Task<bool> RegistrerKode(KodeModell modell);

        Task SendMelding(MeldingModell modell);

        IEnumerable<KodeOutputModel> HentRegistrerteKoder(string lagId);

        Task TildelPoeng(PoengModell modell);
    }
}
