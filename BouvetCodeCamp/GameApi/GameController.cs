namespace BouvetCodeCamp.GameApi
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Domene;
    using Domene.Entiteter;
    using DomeneTjenester.Interfaces;
    using SignalR;

    using Microsoft.AspNet.SignalR;

    [RoutePrefix("api/game")]
    [Obsolete]
    public class GameController : ApiController
    {
        private readonly IGameApi _gameApi;

   //     Lazy<IHubContext<IGameHub>> _gameHub = new Lazy<IHubContext<IGameHub>>(() => GlobalHost.ConnectionManager.GetHubContext<IGameHub>("GameHub"));
        //IHubContext<IGameHub> ?
        Lazy<IHubContext<IGameHub>> _gameHub;
        public GameController(IGameApi gameApi, Lazy<IHubContext<IGameHub>> gameHub)
        {
            _gameApi = gameApi;
            //this._gameHub = gameHub;
        }

        // GET api/game/get
        [Route("get")]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                value = "Welcome to Bouvet Code Camp"
            });
        }

        [HttpGet]
        [Route("sendCommand")]
        public HttpResponseMessage SendCommand(int groupId, Direction direction, double distance, string message)
        {
            // TODO: lagre melding

            return Request.CreateResponse(HttpStatusCode.OK, new
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
            _gameHub.Value.Clients.All.SetRedZone(new Coordinate(model.Longitude,model.Latitude ));
            
            //TODO: opprette infisert sone

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}