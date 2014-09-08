using System.Net.Http;
using BouvetCodeCamp.Models;

namespace BouvetCodeCamp
{
    public interface IGameApi
    {
        HttpResponseMessage RegistrerGeoPosition(GeoPosisjonModel model);
        
        HttpResponseMessage RegistrerKode(KodeModel model);
    }
}
