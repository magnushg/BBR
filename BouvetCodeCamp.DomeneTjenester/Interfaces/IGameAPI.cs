using System.Collections.Generic;
using System.Threading.Tasks;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.Domene.InputModels;
using BouvetCodeCamp.Domene.OutputModels;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    public interface IGameApi
    {
        PifPosisjon RegistrerPifPosition(GeoPosisjonModel model);

        PifPosisjonModel HentSistePifPositionForLag(string lagId);

        IEnumerable<PifPosisjonModel> HentAllePifPosisjoner();

        bool RegistrerKode(KodeModel model);

        void SendMelding(MeldingModel model);
    }
}
