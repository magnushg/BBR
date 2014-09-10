using System.Net;
using System.Net.Http;
using System.Web.Http;
using BouvetCodeCamp.InputModels;

namespace BouvetCodeCamp
{
    [RoutePrefix("api/game")]
    public class GameController : ApiController
    {
       private readonly IGameApi _gameApi;

        [Route("")]
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
        [Route("sendCommand")]
        public HttpResponseMessage SendCommand(int groupId, Direction direction, double distance, string message)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new
           {
                message = string.Format("You chose to move {0} for {1} meters with message {2}", direction, distance, message)
           });
        }

       public HttpResponseMessage RegistrerGeoPosition(string lagId, string lat, string lon)
       {
           return _gameApi.RegistrerGeoPosition(new GeoPosisjonModel
           {
               LagId = lagId,
               Latitude = lat,
               Longditude = lon
           });
       }

       public HttpResponseMessage RegistrerKode(string lagId, string kode)
       {
           return _gameApi.RegistrerKode(new KodeModel
           {
               LagId = lagId,
               Kode = kode
           });
       }

       public HttpResponseMessage SendMelding(string lagId, string tekst, MeldingType type)
       {
           return _gameApi.SendMelding(new MeldingModel
           {
               LagId = lagId,
               Tekst = tekst,
               Type = type
           });
       }

       public HttpResponseMessage HentPifPosisjon(string lagId)
       {
           return _gameApi.HentPifPosisjon(lagId);
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
