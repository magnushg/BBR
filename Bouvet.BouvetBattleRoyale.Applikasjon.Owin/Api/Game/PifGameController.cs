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
    using Bouvet.BouvetBattleRoyale.Domene.OutputModels;
    using Bouvet.BouvetBattleRoyale.Tjenester.Interfaces;

    using BouvetCodeCamp.Domene;

    [RoutePrefix("api/game/pif")]
    public class PifGameController : BaseApiController
    {
        private readonly IGameApi _gameApi;

        private readonly ILagGameService lagGameService;

        public PifGameController(
            IGameApi gameApi,
            ILagGameService lagGameService)
        {
            _gameApi = gameApi;

            this.lagGameService = lagGameService;
        }

        /// <summary>
        /// Tar imot en PIF-posisjon og lagrer posisjonen som siste kjente PIF-posisjon for et lag.
        /// </summary>
        /// <param name="inputModell">PifPosisjonInputModell inputModell</param>
        /// <remarks>POST /api/game/pif/sendpifposisjon</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        [Route("sendpifposisjon")]
        [ResponseType(typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<HttpResponseMessage> SendPifPosisjon([FromBody] PifPosisjonInputModell inputModell)
        {
            if (inputModell == null)
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Modellen er ugyldig");
            
            if (string.IsNullOrEmpty(inputModell.LagId))
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Mangler lagId");
            
            var lag = lagGameService.HentLagMedLagId(inputModell.LagId);

            await _gameApi.RegistrerPifPosisjon(lag, inputModell);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Registrerer en kode på en post for et lag.
        /// </summary>
        /// <param name="inputModell">PostInputModell inputModell</param>
        /// <remarks>POST api/game/pif/sendpostkode</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        [Route("sendpostkode")]
        [ResponseType(typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<HttpResponseMessage> SendPostKode([FromBody] PostInputModell inputModell)
        {
            if (inputModell == null)
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Modellen er ugyldig");
            
            if (string.IsNullOrEmpty(inputModell.LagId))
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Mangler lagId");
            
            var kodeRegistrert = await _gameApi.RegistrerKode(inputModell);

            return kodeRegistrert ?
                Request.CreateResponse(HttpStatusCode.OK) :
                Request.CreateResponse(HttpStatusCode.BadRequest);
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
        public HttpResponseMessage ErInfisert(string lagId)
        {
            if (string.IsNullOrEmpty(lagId))
                OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Modellen er ugyldig");

            var result = _gameApi.ErLagPifInnenInfeksjonssone(lagId);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Henter meldinger som er sendt til PIF.
        ///
        /// MeldingsType.Himmelretning: Innhold kan være 'North', 'South', 'West' eller 'East'. Case-sensitive.
        /// MeldingsType.Stopp: Innhold kan være 'true' eller 'false'. Case-insensitive.
        /// </summary>
        /// <param name="lagId">string lagId</param>
        /// <remarks>GET api/game/pif/hentmeldinger/a-b-c-d</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        [Route("hentmeldinger/{lagId}")]
        [ResponseType(typeof(HttpResponseMessage))]
        [HttpGet]
        public HttpResponseMessage HentMeldinger(string lagId)
        {
            if (string.IsNullOrEmpty(lagId))
                OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Modellen er ugyldig");

            var meldinger = _gameApi.HentMeldinger(lagId);

            var modell = meldinger.Select(melding => new MeldingOutputModell
                                                         {
                                                             LagId = melding.LagId,
                                                             Innhold = melding.Tekst,
                                                             Type = melding.Type.ToString(),
                                                             Tid = melding.Tid
                                                         }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, modell);
        }
    }
}