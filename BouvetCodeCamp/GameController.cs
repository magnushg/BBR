using System.Net;
using System.Net.Http;
using System.Web.Http;
using BouvetCodeCamp.InputModels;

namespace BouvetCodeCamp
{
   public class GameController : ApiController, IGameApi
    {
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                value = "Welcome to Bouvet Code Camp"
            });
        }

       public HttpResponseMessage ReportPosition(int groupId, double lat, double lon)
       {
            return new HttpResponseMessage();    
       }

        [HttpGet]
        [Route("api/game/sendCommand")]
        public HttpResponseMessage SendCommand(int groupId, Direction direction, double distance, string message)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new
           {
                message = string.Format("You chose to move {0} for {1} meters with message {2}", direction, distance, message)
           });
        }

       public HttpResponseMessage RegistrerGeoPosition(GeoPosisjonModel model)
       {
           throw new System.NotImplementedException();
       }

       public HttpResponseMessage RegistrerKode(KodeModel model)
       {
           throw new System.NotImplementedException();
       }

       public HttpResponseMessage SendMelding(MeldingModel model)
       {
           throw new System.NotImplementedException();
       }

       public HttpResponseMessage HentPifPosisjon(string lagId)
       {
           throw new System.NotImplementedException();
       }
    }

    public enum Direction
    {
        North,
        East,
        West,
        South
    }
}
