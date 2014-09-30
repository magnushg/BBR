using System.Collections.Generic;

using BouvetCodeCamp.Domene.InputModels;
using BouvetCodeCamp.Domene.OutputModels;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    using System.Threading.Tasks;

    public interface IGameApi
    {
        void RegistrerPifPosition(PifPosisjonModell modell);

        Domene.OutputModels.PifPosisjonModel HentSistePifPositionForLag(string lagId);
        
        Task<bool> RegistrerKode(KodeModel model);

        Task SendMelding(MeldingModel model);

        IEnumerable<KodeOutputModel> HentRegistrerteKoder(string lagId);
    }
}
