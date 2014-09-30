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
        /// Tar imot en PIF-posisjon og lagrer som siste kjente PIF-posisjon for et lag.
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
                
                this._gameApi.RegistrerPifPosisjon(modell);
            }
            catch (Exception e)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }
        
        /// <summary>
        /// Registrerer en kode på en post for et lag.
        /// </summary>
        /// <param name="modell">KodeModel modell</param>
        /// <remarks>POST api/game/pif/sendpostkode</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        [Route("sendpostkode")]
        [ResponseType(typeof(HttpResponseMessage))]
        [HttpPost]
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
        
        /// <summary>
        /// Henter infisert status for et lag. Er infisert hvis PIF har kommet innenfor en infisert sone.
        /// </summary>
        /// <param name="lagId">string lagId</param>
        /// <remarks>GET api/game/pif/erinfisert/a-b-c-d</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        [Route("erinfisert/{lagId}")]
        [ResponseType(typeof(HttpResponseMessage))]
        [HttpGet]
        public void ErInfisert(string lagId)
        {
           //TODO
        }
    }
}