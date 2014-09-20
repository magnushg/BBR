using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BouvetCodeCamp.Felles;
using BouvetCodeCamp.InputModels;
using MeldingType = BouvetCodeCamp.InputModels.MeldingType;
using Microsoft.AspNet.SignalR;
using BouvetCodeCamp.SignalR;

namespace BouvetCodeCamp
{
    [RoutePrefix("api/game")]
    public class GameController : ApiController
    {
        private readonly IGameApi _gameApi;

        Lazy<IHubContext> _gameHub = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<GameHub>());

        public GameController(IGameApi gameApi)
        {
            _gameApi = gameApi;
            ;

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
        //Øverst til venstre 59.680782, 10.602574
        //Nederst til høyre 59.672267, 10.609526
        [HttpGet]
        [Route("pif/put")]
        public async Task<HttpResponseMessage> RegistrerPifPosition([FromUri] GeoPosisjonModel model)
        {
            var nyPosisjon = await _gameApi.RegistrerPifPosition(model);
            _gameHub.Value.Clients.All.NyPifPosisjon(new OutputModels.PifPosisjonModel { LagId = nyPosisjon.LagId, Latitude = nyPosisjon.Latitude, Longitude = nyPosisjon.Longitude, Tid = nyPosisjon.Tid });
            return Request.CreateResponse(HttpStatusCode.OK, nyPosisjon);
        }

        [HttpGet]
        [Route("pif/get")]
        public async Task<HttpResponseMessage> HentSistePifPositionForLag(string lagId)
        {
            var pifPosisjonModel = await _gameApi.HentSistePifPositionForLag(lagId);

            return Request.CreateResponse(HttpStatusCode.Found, pifPosisjonModel, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("pif/getAll")]
        public async Task<HttpResponseMessage> HentAlleSistePifPosisjoner()
        {
            var pifPosisjonModel = await _gameApi.HentAllePifPosisjoner();

            return Request.CreateResponse(HttpStatusCode.OK, pifPosisjonModel, Configuration.Formatters.JsonFormatter);
        }




        public async Task<HttpResponseMessage> RegistrerKode([FromUri] KodeModel model)
        {
            try
            {
                var kodeRegistrert = await _gameApi.RegistrerKode(model);

                return kodeRegistrert ?
                    Request.CreateResponse(HttpStatusCode.OK) :
                    Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }            

        }




        public HttpResponseMessage SendMelding(string lagId, string tekst, MeldingType type)
        {
            try
            {
                _gameApi.SendMelding(new MeldingModel
                {
                    LagId = lagId,
                    Tekst = tekst,
                    Type = type
                });
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
            return Request.CreateResponse(HttpStatusCode.Created);


        }
    }
}