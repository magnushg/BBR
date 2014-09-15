using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BouvetCodeCamp.Felles.Entiteter;
using BouvetCodeCamp.InputModels;

namespace BouvetCodeCamp
{
    [RoutePrefix("api/game")]
    public class GameController : ApiController
    {
       private readonly IGameApi _gameApi;

       public GameController(IGameApi gameApi)
       {
           _gameApi = gameApi;
       }
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

        [HttpGet]
        [Route("pif/put")]
        public async Task<HttpResponseMessage> RegistrerPifPosition([FromUri] GeoPosisjonModel model)
        {
            return await _gameApi.RegistrerPifPosition(model);
        }

        [HttpGet]
        [Route("pif/get")]
        public async Task<HttpResponseMessage> HentSistePifPositionForLag(string lagId)
        {
            var pifPosisjonModel = await _gameApi.HentSistePifPositionForLag(lagId);

            return Request.CreateResponse(HttpStatusCode.Found, pifPosisjonModel, Configuration.Formatters.JsonFormatter);
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
}
