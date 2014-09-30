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
    using BouvetCodeCamp.Domene.OutputModels;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;
    using BouvetCodeCamp.SignalR;

    using Microsoft.AspNet.SignalR;

    using PifPosisjonModell = BouvetCodeCamp.Domene.InputModels.PifPosisjonModell;

    [RoutePrefix("api/game/pif")]
    public class PifGameController : BaseApiController
    {
        private readonly IGameApi _gameApi;

        readonly Lazy<IHubContext<IGameHub>> _gameHub;

        public PifGameController(IGameApi gameApi, Lazy<IHubContext<IGameHub>> gameHub)
        {
            _gameApi = gameApi;
            _gameHub = gameHub;
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
        public async Task<HttpResponseMessage> SendPifPosisjon([FromBody] PifPosisjonModell modell)
        {
            if (modell == null) 
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat);

            try
            {
                this._gameHub.Value.Clients.All.NyPifPosisjon(
                    new Domene.OutputModels.PifPosisjonModell
                        {
                            LagId = modell.LagId, 
                            Latitude = modell.Latitude, 
                            Longitude = modell.Longitude, 
                            Tid = DateTime.Now
                        });
                
                await this._gameApi.RegistrerPifPosisjon(modell);
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
        public async Task<HttpResponseMessage> SendPostKode([FromBody] KodeModell modell)
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