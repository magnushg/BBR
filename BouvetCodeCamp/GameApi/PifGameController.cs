namespace BouvetCodeCamp.GameApi
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using BouvetCodeCamp.Domene;
    using BouvetCodeCamp.Domene.InputModels;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;

    [RoutePrefix("api/game/pif")]
    public class PifGameController : BaseApiController
    {
        private readonly IGameApi _gameApi;

        public PifGameController(IGameApi gameApi)
        {
            _gameApi = gameApi;
        }

        /// <summary>
        /// Tar imot en PIF-posisjon og lagrer som siste kjente posisjon.
        /// </summary>
        /// <param name="modell">PifPosisjonModell modell</param>
        /// <remarks>POST /api/game/pif/sendpifposisjon</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        [Route("sendpifposisjon")]
        [ResponseType(typeof(HttpResponseMessage))]
        [HttpPost]
        public HttpResponseMessage SendPifPosisjon([FromBody] PifPosisjonModell modell)
        {
            if (modell == null) 
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat);

            try
            {
                //this._gameHub.Clients.All.NyPifPosisjon(new PifPosisjonModel { LagId = modell.LagId, Latitude = modell.Latitude, Longitude = modell.Longitude, Tid = DateTime.Now });
                
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
        public async Task<HttpResponseMessage> SendPostKode([FromBody] KodeModel modell)
        {
            if (modell == null)
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat);

            try
            {
                var kodeRegistrert = await _gameApi.RegistrerKode(modell);

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
           //TODO
        }
    }
}