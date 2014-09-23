using System.Collections.Generic;
using System.Threading.Tasks;
using BouvetCodeCamp.Felles.Entiteter;
using BouvetCodeCamp.InputModels;
using BouvetCodeCamp.OutputModels;

namespace BouvetCodeCamp
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
