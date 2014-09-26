namespace BouvetCodeCamp.GameApi
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using BouvetCodeCamp.Domene.InputModels;
    using BouvetCodeCamp.Domene.OutputModels;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;
    using BouvetCodeCamp.SignalR;

    using Microsoft.AspNet.SignalR;

    [RoutePrefix("api/game/pif")]
    public class PifGameController : ApiController
    {
        private readonly IGameApi _gameApi;
        Lazy<IHubContext<IGameHub>> _gameHub;

        public PifGameController(IGameApi gameApi)
        {
            this._gameApi = gameApi;
        }

        // PUT api/game/pif/put
        [HttpGet]
        [Route("put")]
        public HttpResponseMessage PutPifPosition([FromUri] GeoPosisjonModel model)
        {
            //Øverst til venstre 59.680782, 10.602574
            //Nederst til høyre 59.672267, 10.609526

            this._gameHub.Value.Clients.All.NyPifPosisjon(new PifPosisjonModel { LagId = model.LagId, Latitude = model.Latitude, Longitude = model.Longitude, Tid = DateTime.Now });
            var nyPosisjon = this._gameApi.RegistrerPifPosition(model);
           
            return this.Request.CreateResponse(HttpStatusCode.OK, nyPosisjon);
        }

        // GET api/game/pif/get/91735
        [HttpGet]
        [Route("get/{lagId}")]
        public HttpResponseMessage GetPifPosisjon(string lagId)
        {
            var pifPosisjonModel = this._gameApi.HentSistePifPositionForLag(lagId);

            return this.Request.CreateResponse(HttpStatusCode.Found, pifPosisjonModel, this.Configuration.Formatters.JsonFormatter);
        }

        // GET api/game/pif/get
        [HttpGet]
        [Route("get")]
        public HttpResponseMessage Get()
        {
            var pifPosisjonModel = this._gameApi.HentAllePifPosisjoner();

            return this.Request.CreateResponse(HttpStatusCode.OK, pifPosisjonModel, this.Configuration.Formatters.JsonFormatter);
        }
    }
}