namespace Bouvet.BouvetBattleRoyale.Applikasjon.Owin.Api.Admin
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Bouvet.BouvetBattleRoyale.Applikasjon.Owin;
    using Bouvet.BouvetBattleRoyale.Domene;
    using Bouvet.BouvetBattleRoyale.Domene.InputModels;
    using Bouvet.BouvetBattleRoyale.Domene.OutputModels;
    using Bouvet.BouvetBattleRoyale.Tjenester.SignalR.Hubs;

    using BouvetCodeCamp.Domene;
    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;

    [RoutePrefix("api/admin/lag")]
    [System.Web.Http.Authorize]
    public class LagController : BaseApiController
    {
        private readonly IService<Lag> lagService;

        private readonly IGameApi gameApi;

        private readonly IGameHub gameHub;

        private readonly ILagGameService lagGameService;

        public LagController(
            IService<Lag> lagService,
            IGameApi gameApi,
            IGameHub gameHub,
            ILagGameService lagGameService)
        {
            this.lagService = lagService;
            this.gameApi = gameApi;
            this.gameHub = gameHub;
            this.lagGameService = lagGameService;
        }

        // GET api/admin/lag/get
        [Route("get")]
        [HttpGet]
        [Obsolete] // Skjule for Swagger-apidoc
        public HttpResponseMessage Get()
        {
            var alleLag = lagService.HentAlle();

            return Request.CreateResponse(HttpStatusCode.OK, alleLag);
        }

        // GET api/admin/lag/get/a-b-c-d
        [Route("get/{id}")]
        [HttpGet]
        [Obsolete] // Skjule for Swagger-apidoc
        public HttpResponseMessage GetLag(string id)
        {
            if (string.IsNullOrEmpty(id))
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Mangler id");

            var lag = lagService.Hent(id);

            if (lag == null)
                return OpprettErrorResponse(
                    ErrorResponseType.FantIkkeObjekt,
                    string.Format("Lag med id = '{0}' fantes ikke.", id));

            return Request.CreateResponse(HttpStatusCode.OK, lag);
        }

        // POST api/admin/lag/post
        [Route("post")]
        [HttpPost]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> PostLag([FromBody]Lag modell)
        {
            try
            {
                if (modell == null) 
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig request");

                await lagService.Opprett(modell);

            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // PUT api/admin/lag/put
        [Route("put")]
        [HttpPut]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> PutLag([FromBody]Lag modell)
        {
            if (modell == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig request");

            await lagService.Oppdater(modell);
            
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/admin/lag/delete
        [Route("delete")]
        [HttpDelete]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> Delete()
        {
            var alleLag = lagService.HentAlle();

            foreach (var lag in alleLag)
            {
                await lagService.Slett(lag);
            }
            
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/admin/lag/delete/a-b-c-d
        [Route("delete/{id}")]
        [HttpDelete]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> DeleteLag(string id)
        {
            if (string.IsNullOrEmpty(id))
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Mangler id");

            var lag = lagService.Hent(id);

            if (lag == null)
                return OpprettErrorResponse(
                    ErrorResponseType.FantIkkeObjekt, 
                    string.Format("Lag med id = '{0}' fantes ikke.", id));
            
            await lagService.Slett(lag);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/admin/lag/deletebylagid/a-b-c-d
        [Route("deletebylagid/{lagId}")]
        [HttpDelete]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> DeleteByLagId(string lagId)
        {
            if (string.IsNullOrEmpty(lagId))
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Mangler LagId");

            var lagTilSletting = lagService.Sok(o => o.LagId == lagId);
            
            foreach (var lag in lagTilSletting)
            {
                await lagService.Slett(lag);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/admin/lag/tildelpoeng
        [Route("tildelpoeng")]
        [HttpPost]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> TildelPoeng([FromBody]PoengInputModell inputModell)
        {
            if (inputModell == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig request");

            if (string.IsNullOrEmpty(inputModell.LagId))
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Mangler LagId");

            await gameApi.TildelPoeng(inputModell);
            
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/admin/lag/oppretthendelse
        [Route("oppretthendelse")]
        [HttpPost]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> OpprettHendelse([FromBody]LoggHendelseInputModell inputModell)
        {
            if (inputModell == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig request");

            if (string.IsNullOrEmpty(inputModell.LagId))
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Mangler LagId");

            await gameApi.OpprettHendelse(inputModell.LagId, inputModell.HendelseType, inputModell.Kommentar);

            var lag = lagGameService.HentLagMedLagId(inputModell.LagId);

            gameHub.NyLoggHendelse(
                new LoggHendelseOutputModell
                {
                    LagNummer = lag.LagNummer,
                    Hendelse = HendelseTypeFormatter.HentTekst(inputModell.HendelseType),
                    Kommentar = inputModell.Kommentar,
                    Tid = DateTime.Now.ToLongTimeString(),
                    LagId = lag.LagId
                });

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}