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
            //this._gameHub = gameHub;
        }

        // GET api/game/get
        [Route("get")]
        public HttpResponseMessage Get()
        {
            return this.Request.CreateResponse(HttpStatusCode.OK, new
            {
                value = "Welcome to Bouvet Code Camp"
            });
        }

        [HttpGet]
        [Route("sendCommand")]
        public HttpResponseMessage SendCommand(int groupId, Direction direction, double distance, string message)
        {
            // TODO: lagre melding

            return this.Request.CreateResponse(HttpStatusCode.OK, new
           {
               message = string.Format("You chose to move {0} for {1} meters with message {2}", direction, distance, message)
           });
        }
       
        //Øverst til venstre 59.680782, 10.602574
        //Nederst til høyre 59.672267, 10.609526
        [HttpGet]
        [Route("setRedZone")]
        public async Task<HttpResponseMessage> SetRedZone([FromUri] Coordinate model)
        {
            this._gameHub.Value.Clients.All.SetRedZone(new Coordinate(model.Longitude,model.Latitude ));
            
            //TODO: opprette infisert sone

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }
        
        public HttpResponseMessage RegistrerKode([FromUri] KodeModel model)
        {
            try
            {
                var kodeRegistrert = this._gameApi.RegistrerKode(model);

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