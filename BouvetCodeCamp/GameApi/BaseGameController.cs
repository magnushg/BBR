namespace BouvetCodeCamp.GameApi
{
    using System;
    using System.Net;
    using System.Net.Http;
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

        // GET api/game/base/hentregistrertekoder
        [HttpGet]
        [Route("hentregistrertekoder")]
        public void HentRegistrerteKoder()
        {
            
        }

        // GET api/game/base/hentPifPosisjon/91735
        [HttpGet]
        [Route("hentpifposisjon/{lagId}")]
        public HttpResponseMessage HentPifPosisjon(string lagId)
        {
            if (string.IsNullOrEmpty(lagId)) 
                OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat);

            var pifPosisjonModel = gameApi.HentSistePifPositionForLag(lagId);

            return Request.CreateResponse(HttpStatusCode.Found, pifPosisjonModel, Configuration.Formatters.JsonFormatter);
        }

        // GET api/game/base/hentgjeldendepost
        [HttpGet]
        [Route("hentgjeldendepost")]
        public void HentGjeldendePost()
        {
            
        }
        
        // POST api/game/base/sendPifMelding
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