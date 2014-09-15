using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BouvetCodeCamp.InputModels;
using BouvetCodeCamp.OutputModels;

namespace BouvetCodeCamp
{
    public interface IGameApi
    {
        Task<HttpResponseMessage> RegistrerPifPosition(GeoPosisjonModel model);

        Task<PifPosisjonModel>HentSistePifPositionForLag(string lagId);

        Task<IEnumerable<PifPosisjonModel>> HentAllePifPosisjoner();
        
        HttpResponseMessage RegistrerKode(KodeModel model);

        HttpResponseMessage SendMelding(MeldingModel model);

        HttpResponseMessage HentPifPosisjon(string lagId);
    }
}
