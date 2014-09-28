namespace BouvetCodeCamp.GameApi
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using BouvetCodeCamp.Domene;
    using BouvetCodeCamp.Domene.InputModels;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;
    using BouvetCodeCamp.SignalR;

    using Microsoft.AspNet.SignalR;

    [RoutePrefix("api/game/pif")]
    public class PifGameController : BaseApiController
    {
        private readonly IGameApi _gameApi;
        Lazy<IHubContext<IGameHub>> _gameHub;

        public PifGameController(IGameApi gameApi)
        {
            _gameApi = gameApi;
        }

        // POST api/game/pif/SendPifPosition
        [HttpPost]
        [Route("sendpifposition")]
        public HttpResponseMessage SendPifPosition([FromBody] GeoPosisjonModel modell)
        {
            if (modell == null) 
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat);

            try
            {
                //TODO: Morten u fix?
                //this._gameHub.Value.Clients.All.NyPifPosisjon(new PifPosisjonModel { LagId = modell.LagId, Latitude = modell.Latitude, Longitude = modell.Longitude, Tid = DateTime.Now });
                this._gameApi.RegistrerPifPosition(modell);
            }
            catch (Exception e)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }
        
        // POST api/game/pif/sendpostkode
        [HttpPost]
        [Route("sendpostkode")]
        public HttpResponseMessage SendPostKode([FromBody] KodeModel modell)
        {
            if (modell == null)
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat);

            try
            {
                var kodeRegistrert = _gameApi.RegistrerKode(modell);

                return kodeRegistrert ?
                    Request.CreateResponse(HttpStatusCode.OK) :
                    Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        // GET api/game/pif/erinfisert
        [HttpGet]
        [Route("erinfisert")]
        public void ErInfisert()
        {
           
        }
    }
}