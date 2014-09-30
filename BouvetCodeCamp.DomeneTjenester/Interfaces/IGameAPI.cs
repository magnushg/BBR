using System.Collections.Generic;

using BouvetCodeCamp.Domene.InputModels;
using BouvetCodeCamp.Domene.OutputModels;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    using System.Threading.Tasks;

    public interface IGameApi
    {
        Task RegistrerPifPosisjon(PifPosisjonModell modell);

        PifPosisjonModel HentSistePifPositionForLag(string lagId);
        
        Task<bool> RegistrerKode(KodeModel model);

        Task SendMelding(MeldingModell modell);

        IEnumerable<KodeOutputModel> HentRegistrerteKoder(string lagId);
    }
}
