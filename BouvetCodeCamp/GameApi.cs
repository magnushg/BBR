using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouvetCodeCamp
{
    public class GameApi:IGameApi
    {
        public System.Net.Http.HttpResponseMessage RegistrerGeoPosition(InputModels.GeoPosisjonModel model)
        {
            throw new NotImplementedException();
        }

        public System.Net.Http.HttpResponseMessage RegistrerKode(InputModels.KodeModel model)
        {
            throw new NotImplementedException();
        }

        public System.Net.Http.HttpResponseMessage SendMelding(InputModels.MeldingModel model)
        {
            throw new NotImplementedException();
        }

        public System.Net.Http.HttpResponseMessage HentPifPosisjon(string lagId)
        {
            throw new NotImplementedException();
        }
    }
}
