namespace BouvetCodeCamp.GameApi
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using BouvetCodeCamp.Domene;
    using BouvetCodeCamp.Domene.InputModels;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;
    
    [RoutePrefix("api/game/base")]
    public class BaseGameController : ApiController
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
                this.OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat);

            var pifPosisjonModel = this.gameApi.HentSistePifPositionForLag(lagId);

            return this.Request.CreateResponse(HttpStatusCode.Found, pifPosisjonModel, this.Configuration.Formatters.JsonFormatter);
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
                return this.OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat);

            try
            {
                this.gameApi.SendMelding(modell);
            }
            catch (Exception e)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
            return this.Request.CreateResponse(HttpStatusCode.Created);
        }

        private HttpResponseMessage OpprettErrorResponse(ErrorResponseType errorResponseType)
        {
            switch (errorResponseType)
            {
                case ErrorResponseType.UgyldigInputFormat:
                    return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig inputformat");
            }
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig forespørsel");
        }
    }
}