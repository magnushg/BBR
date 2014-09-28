using System.Collections.Generic;

using BouvetCodeCamp.Domene.InputModels;
using BouvetCodeCamp.Domene.OutputModels;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    public interface IGameApi
    {
        void RegistrerPifPosition(GeoPosisjonModel model);

        PifPosisjonModel HentSistePifPositionForLag(string lagId);
        
        bool RegistrerKode(KodeModel model);

        void SendMelding(MeldingModel model);

        IEnumerable<KodeOutputModel> HentRegistrerteKoder(string lagId);
    }
}
