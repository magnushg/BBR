namespace Bouvet.BouvetBattleRoyale.Applikasjon.Owin.Api.Game
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Bouvet.BouvetBattleRoyale.Applikasjon.Owin;
    using Bouvet.BouvetBattleRoyale.Domene;
    using Bouvet.BouvetBattleRoyale.Domene.InputModels;
    using Bouvet.BouvetBattleRoyale.Tjenester.Interfaces;

    using BouvetCodeCamp.Domene;

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
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Mangler lagId");

            var kodeModeller = gameApi.HentRegistrerteKoder(lagId);

            return Request.CreateResponse(
                HttpStatusCode.OK,
                kodeModeller.Select(x => x.Kode),
                Configuration.Formatters.JsonFormatter);
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
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Mangler lagId");

            var pifPosisjonModel = gameApi.HentSistePifPositionForLag(lagId);

            return Request.CreateResponse(HttpStatusCode.OK, pifPosisjonModel, Configuration.Formatters.JsonFormatter);
        }

        /// <summary>
        /// Henter gjeldende post for et lag. 
        /// Hvis det ikke er flere uoppdagede poster igjen returneres NULL.
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
            if (string.IsNullOrEmpty(lagId))
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Mangler lagId");

            return Request.CreateResponse(HttpStatusCode.OK, gameApi.HentGjeldendePost(lagId));
        }

        /// <summary>
        /// Sender en melding til PIF. 
        /// Det følger enkelte restriksjoner på Innhold-feltet basert på 'MeldingsType' (husk stor forbokstav!):
        /// 
        /// MeldingsType.Ingen 
        /// MeldingsType.Fritekst: Innhold.Length må være mindre eller lik 256 tegn.
        /// MeldingsType.Lengde: Innhold må være en Int
        /// MeldingsType.Himmelretning: Innhold kan være 'North', 'South', 'West' eller 'East'. Case-sensitive.
        /// MeldingsType.Stopp: Innhold kan være 'true' eller 'false'.
        /// </summary>
        /// <param name="inputModell">MeldingInputModell inputModell</param>
        /// <remarks>POST api/game/base/sendpifmelding</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        [Route("sendpifmelding")]
        [ResponseType(typeof(HttpResponseMessage))]
        [HttpPost]
        public virtual async Task<HttpResponseMessage> SendPifMelding([FromBody] MeldingInputModell inputModell)
        {
            if (inputModell == null)
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Modellen er ugyldig");

            if (string.IsNullOrEmpty(inputModell.LagId))
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Mangler lagId");
            
            try
            {
                await gameApi.SendMelding(inputModell);
            }
            catch (MeldingException msgException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, msgException);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}