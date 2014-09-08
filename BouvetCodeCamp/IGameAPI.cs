using System.Net.Http;
using BouvetCodeCamp.InputModels;

namespace BouvetCodeCamp
{
    public interface IGameApi
    {
        HttpResponseMessage RegistrerGeoPosition(GeoPosisjonModel model);
        
        HttpResponseMessage RegistrerKode(KodeModel model);

        HttpResponseMessage SendMelding(MeldingModel model);

        HttpResponseMessage HentPifPosisjon(string lagId);
    }
}
