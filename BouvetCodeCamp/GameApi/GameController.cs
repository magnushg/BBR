namespace BouvetCodeCamp.GameApi
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using BouvetCodeCamp.Domene;
    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.Domene.InputModels;
    using BouvetCodeCamp.Domene.OutputModels;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;
    using BouvetCodeCamp.SignalR;

    using Microsoft.AspNet.SignalR;

    using MeldingType = BouvetCodeCamp.Domene.InputModels.MeldingType;

    [RoutePrefix("api/game")]
    public class GameController : ApiController
    {
        private readonly IGameApi _gameApi;

   //     Lazy<IHubContext<IGameHub>> _gameHub = new Lazy<IHubContext<IGameHub>>(() => GlobalHost.ConnectionManager.GetHubContext<IGameHub>("GameHub"));
        //IHubContext<IGameHub> ?
        Lazy<IHubContext<IGameHub>> _gameHub;
        public GameController(IGameApi gameApi, Lazy<IHubContext<IGameHub>> gameHub)
        {
            this._gameApi = gameApi;
            this._gameHub = gameHub;
        }
        [Route("")]
        public HttpResponseMessage Get()
        {
            return this.Request.CreateResponse(HttpStatusCode.OK, new
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
            return this.Request.CreateResponse(HttpStatusCode.OK, new
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

            this._gameHub.Value.Clients.All.NyPifPosisjon(new PifPosisjonModel { LagId = model.LagId, Latitude = model.Latitude, Longitude = model.Longitude, Tid = DateTime.Now });
            var nyPosisjon = await this._gameApi.RegistrerPifPosition(model);
           
            return this.Request.CreateResponse(HttpStatusCode.OK, nyPosisjon);
        }

        //Øverst til venstre 59.680782, 10.602574
        //Nederst til høyre 59.672267, 10.609526
        [HttpGet]
        [Route("setRedZone")]
        public async Task<HttpResponseMessage> SetRedZone([FromUri] Coordinate model)
        {

            this._gameHub.Value.Clients.All.SetRedZone(new Coordinate(model.Longitude,model.Latitude ));
            
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("pif/get")]
        public async Task<HttpResponseMessage> HentSistePifPositionForLag(string lagId)
        {
            var pifPosisjonModel = await this._gameApi.HentSistePifPositionForLag(lagId);

            return this.Request.CreateResponse(HttpStatusCode.Found, pifPosisjonModel, this.Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("pif/getAll")]
        public async Task<HttpResponseMessage> HentAlleSistePifPosisjoner()
        {
            var pifPosisjonModel = await this._gameApi.HentAllePifPosisjoner();

            return this.Request.CreateResponse(HttpStatusCode.OK, pifPosisjonModel, this.Configuration.Formatters.JsonFormatter);
        }




        public async Task<HttpResponseMessage> RegistrerKode([FromUri] KodeModel model)
        {
            try
            {
                var kodeRegistrert = await this._gameApi.RegistrerKode(model);

                return kodeRegistrert ?
                    this.Request.CreateResponse(HttpStatusCode.OK) :
                    this.Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }            

        }




        public HttpResponseMessage SendMelding(string lagId, string tekst, MeldingType type)
        {
            try
            {
                this._gameApi.SendMelding(new MeldingModel
                {
                    LagId = lagId,
                    Tekst = tekst,
                    Type = type
                });
            }
            catch (Exception e)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
            return this.Request.CreateResponse(HttpStatusCode.Created);


        }
    }
}