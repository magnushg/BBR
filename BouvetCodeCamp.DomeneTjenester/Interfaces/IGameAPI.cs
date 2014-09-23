using System.Collections.Generic;
using System.Threading.Tasks;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.Domene.InputModels;
using BouvetCodeCamp.Domene.OutputModels;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    public interface IGameApi
    {
        Task<PifPosisjon> RegistrerPifPosition(GeoPosisjonModel model);

        Task<PifPosisjonModel> HentSistePifPositionForLag(string lagId);

        Task<IEnumerable<PifPosisjonModel>> HentAllePifPosisjoner();

        Task<bool> RegistrerKode(KodeModel model);

        void SendMelding(MeldingModel model);
    }
}
