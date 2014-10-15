namespace BouvetCodeCamp.Api.Game
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using BouvetCodeCamp.Domene.Entiteter;

    using SignalR.Hubs;

    using Domene;
    using Domene.InputModels;
    using Domene.OutputModels;
    using DomeneTjenester.Interfaces;
    using Microsoft.AspNet.SignalR;

    [RoutePrefix("api/game/pif")]
    public class PifGameController : BaseApiController
    {
        private readonly IGameApi _gameApi;

        readonly Lazy<IHubContext<IGameHub>> _gameHub;

        private readonly ILagGameService lagGameService;

        public PifGameController(
            IGameApi gameApi, 
            Lazy<IHubContext<IGameHub>> gameHub,
            ILagGameService lagGameService)
        {
            _gameApi = gameApi;
            _gameHub = gameHub;
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

            try
            {
                var lag = lagGameService.HentLagMedLagId(inputModell.LagId);
                
                await _gameApi.RegistrerPifPosisjon(lag, inputModell);

                var erInfisert = _gameApi.ErInfisiert(inputModell.Posisjon);
                
                _gameHub.Value.Clients.All.NyPifPosisjon(
                    new PifPosisjonOutputModell
                        {
                            LagId = inputModell.LagId,
                            Latitude = inputModell.Posisjon.Latitude,
                            Longitude = inputModell.Posisjon.Longitude,
                            Tid = DateTime.Now,
                            ErInfisert = erInfisert
                        });

                _gameHub.Value.Clients.All.NyLoggHendelse(
                    new LoggHendelseOutputModell
                    {
                        LagNummer = lag.LagNummer,
                        Hendelse = HendelseTypeFormatter.HentTekst(HendelseType.RegistrertPifPosisjon),
                        Kommentar = erInfisert ? "ER I INFISERT SONE" : string.Empty,
                        Tid = DateTime.Now.ToLongTimeString()
                    });
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Registrerer en kode p￥ en post for et lag.
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

            try
            {
                var kodeRegistrert = await _gameApi.RegistrerKode(inputModell);

                return kodeRegistrert ?
                    Request.CreateResponse(HttpStatusCode.OK) :
                    Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
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
        public HttpResponseMessage ErInfisert(string lagId)
        {
            if (string.IsNullOrEmpty(lagId))
                OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Modellen er ugyldig");

            try
            {
                var result = _gameApi.ErLagPifInnenInfeksjonssone(lagId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Henter meldinger som er sendt til PIF.
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

            try
            {
                var meldinger = _gameApi.HentMeldinger(lagId);

                var modell = meldinger.Select(melding => new MeldingOutputModell
                                                             {
                                                                 LagId = melding.LagId,
                                                                 Tekst = melding.Tekst,
                                                                 Type = melding.Type,
                                                                 Tid = melding.Tid
                                                             }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, modell);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}