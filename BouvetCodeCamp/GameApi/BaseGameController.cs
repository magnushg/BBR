namespace BouvetCodeCamp.GameApi
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Domene;
    using Domene.InputModels;
    using DomeneTjenester.Interfaces;

    [RoutePrefix("api/game/base")]
    public class BaseGameController : BaseApiController
    {
        private readonly IGameApi gameApi;

        public BaseGameController(IGameApi gameApi)
        {
            this.gameApi = gameApi;
        }
        
        /// <summary>
        /// Henter alle kodene som et lag har registrert ute på postene.
        /// </summary>
        /// <param name="lagId">string lagId</param>
        /// <remarks>GET api/game/base/hentregistrertekoder/142</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        [Route("hentregistrertekoder/{lagId}")]
        [ResponseType(typeof(HttpResponseMessage))]
        [HttpGet]
        public HttpResponseMessage HentRegistrerteKoder(string lagId)
        {
            if (string.IsNullOrEmpty(lagId))
                OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat);

            try
            {
                var kodeModeller = gameApi.HentRegistrerteKoder(lagId);

                return Request.CreateResponse(HttpStatusCode.OK, kodeModeller, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }
        
        /// <summary>
        /// Henter siste kjente posisjon for PIF.
        /// </summary>
        /// <param name="lagId">string lagId</param>
        /// <remarks>GET api/game/base/hentpifposisjon/91735</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        [Route("hentpifposisjon/{lagId}")]
        [ResponseType(typeof(HttpResponseMessage))]
        [HttpGet]
        public HttpResponseMessage HentPifPosisjon(string lagId)
        {
            if (string.IsNullOrEmpty(lagId)) 
                OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat);

            try
            {
                var pifPosisjonModel = gameApi.HentSistePifPositionForLag(lagId);

                return Request.CreateResponse(HttpStatusCode.OK, pifPosisjonModel, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }
        
        /// <summary>
        /// Henter gjeldende post for et lag.
        /// </summary>
        /// <param name="lagId">string lagId</param>
        /// <remarks>GET api/game/base/hentgjeldendepost/a-b-c-d-</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        [Route("hentgjeldendepost/{lagId}")]
        [ResponseType(typeof(HttpResponseMessage))]
        [HttpGet]
        public HttpResponseMessage HentGjeldendePost(string lagId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, gameApi.HentGjeldendePost(lagId));
        }
        
        /// <summary>
        /// Sender en melding til PIF.
        /// </summary>
        /// <param name="modell">MeldingModell modell</param>
        /// <remarks>POST api/game/base/sendpifmelding</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        [Route("sendpifmelding")]
        [ResponseType(typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<HttpResponseMessage> SendPifMelding([FromBody] MeldingModell modell)
        {
            if (modell == null) 
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat);

            try
            {
                await gameApi.SendMelding(modell);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}