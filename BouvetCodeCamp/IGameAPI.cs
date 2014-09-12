using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BouvetCodeCamp.InputModels;
using BouvetCodeCamp.OutputModels;

namespace BouvetCodeCamp
{
    public interface IGameApi
    {
        HttpResponseMessage RegistrerPifPosition(GeoPosisjonModel model);

        Task<PifPosisjonModel>GetPifPosition(string lagId);

        Task<IEnumerable<PifPosisjonModel>> GetAllPifPositions();
        
        HttpResponseMessage RegistrerKode(KodeModel model);

        HttpResponseMessage SendMelding(MeldingModel model);

        HttpResponseMessage HentPifPosisjon(string lagId);
    }
}
