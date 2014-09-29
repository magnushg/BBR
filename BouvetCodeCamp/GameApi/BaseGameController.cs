namespace BouvetCodeCamp.GameApi
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

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

        // GET api/game/base/hentregistrertekoder/142
        [HttpGet]
        [Route("hentregistrertekoder/{lagId}")]
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

        // GET api/game/base/hentpifposisjon/91735
        [HttpGet]
        [Route("hentpifposisjon/{lagId}")]
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

        // GET api/game/base/hentgjeldendepost/a-b-c-d-
        [HttpGet]
        [Route("hentgjeldendepost/{lagId}")]
        public void HentGjeldendePost(string lagId)
        {
            //TODO: finn alle postene til laget. Plukk neste fra listen som ikke er Registrert.
        }

        // POST api/game/base/sendpifmelding
        [HttpPost]
        [Route("sendpifmelding")]
        public HttpResponseMessage SendPifMelding([FromBody] MeldingModel modell)
        {
            if (modell == null) 
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat);

            try
            {
                gameApi.SendMelding(modell);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }
}