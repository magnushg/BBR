namespace BouvetCodeCamp.AdminApi
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using BouvetCodeCamp.Domene;
    using BouvetCodeCamp.Domene.InputModels;
    using BouvetCodeCamp.GameApi;

    using Domene.Entiteter;
    using DomeneTjenester.Interfaces;

    [RoutePrefix("api/lag")]
    [Authorize]
    public class LagController : BaseApiController
    {
        private readonly ILagService lagService;

        private readonly IGameApi gameApi;

        public LagController(ILagService lagService, IGameApi gameApi)
        {
            this.lagService = lagService;
            this.gameApi = gameApi;
        }

        // GET api/lag/get
        [Route("get")]
        [HttpGet]
        [Obsolete] // Skjule for Swagger-apidoc
        public HttpResponseMessage Get()
        {
            var alleLag = lagService.HentAlleLag();
            
            return Request.CreateResponse(HttpStatusCode.OK, alleLag);
        }

        // GET api/lag/get/a-b-c-d
        [Route("get/{id}")]
        [HttpGet]
        [Obsolete] // Skjule for Swagger-apidoc
        public HttpResponseMessage GetLag(string id)
        {
            if (string.IsNullOrEmpty(id))
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Mangler id");

            var lag = lagService.Hent(id);

            if (lag == null)
                return this.OpprettErrorResponse(
                    ErrorResponseType.FantIkkeObjekt,
                    string.Format("Lag med id = '{0}' fantes ikke.", id));

            return Request.CreateResponse(HttpStatusCode.OK, lag);
        }

        // POST api/lag/post
        [Route("post")]
        [HttpPost]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> PostLag([FromBody]Lag modell)
        {
            if (modell == null) 
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig request");

            await lagService.Opprett(modell);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // PUT api/lag/put
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

        // DELETE api/lag/delete
        [Route("delete")]
        [HttpDelete]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> Delete()
        {
            await lagService.SlettAlle();
            
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/lag/delete/a-b-c-d
        [Route("delete/{id}")]
        [HttpDelete]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> DeleteLag(string id)
        {
            if (string.IsNullOrEmpty(id))
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Mangler id");

            var lag = lagService.Hent(id);

            if (lag == null)
                return this.OpprettErrorResponse(
                    ErrorResponseType.FantIkkeObjekt, 
                    string.Format("Lag med id = '{0}' fantes ikke.", id));
            
            await lagService.Slett(lag);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/lag/deletebylagid/a-b-c-d
        [Route("deletebylagid/{lagId}")]
        [HttpDelete]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> DeleteByLagId(string lagId)
        {
            if (string.IsNullOrEmpty(lagId))
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Mangler LagId");

            var lagTilSletting = lagService.Søk(o => o.LagId == lagId);
            
            foreach (var lag in lagTilSletting)
            {
                await lagService.Slett(lag);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        
        // POST api/lag/tildelpoeng
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
    }
}