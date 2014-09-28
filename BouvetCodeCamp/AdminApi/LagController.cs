namespace BouvetCodeCamp.AdminApi
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Domene.Entiteter;
    using DomeneTjenester.Interfaces;

    [RoutePrefix("api/lag")]
    [Authorize]
    public class LagController : ApiController
    {
        private readonly IRepository<Lag> lagRepository;

        public LagController(IRepository<Lag> lagRepository)
        {
            this.lagRepository = lagRepository;
        }

        // GET api/lag/get
        [Route("get")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var lagene = lagRepository.HentAlle();

            if (lagene == null || !lagene.Any()) 
                return OpprettIngenLagFantesIkkeResponse();

            return Request.CreateResponse(HttpStatusCode.OK, lagene);
        }

        // GET api/lag/get/a-b-c-d
        [Route("get/{id}")]
        [HttpGet]
        public HttpResponseMessage GetLag(string id)
        {
            var lag = lagRepository.Hent(id);

            if (lag == null)
            {
                return OpprettLagFantesIkkeResponse(id);
            }

            return Request.CreateResponse(HttpStatusCode.OK, lag);
        }

        // POST api/lag/post
        [Route("post")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostLag([FromBody]Lag model)
        {
            if (model == null) 
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig request");

            await lagRepository.Opprett(model);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // PUT api/lag/put
        [Route("put")]
        [HttpPut]
        public async Task<HttpResponseMessage> PutLag([FromBody]Lag model)
        {
            if (model == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig request");

            await lagRepository.Oppdater(model);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/lag/delete
        [Route("delete")]
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete()
        {
            await lagRepository.SlettAlle();
            
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/lag/delete/a-b-c-d
        [Route("delete/{id}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(string id)
        {
            var lag = lagRepository.Hent(id);

            if (lag == null)
                return OpprettLagFantesIkkeResponse(id);

            await lagRepository.Slett(lag);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/lag/delete/a-b-c-d
        [Route("deletelag/{lagId}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteLag(string lagId)
        {
            var lagTilSletting = lagRepository.Søk(o => o.LagId == lagId);
            
            foreach (var lag in lagTilSletting)
            {
                await lagRepository.Slett(lag);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [NonAction]
        private HttpResponseMessage OpprettLagFantesIkkeResponse(string id)
        {
            var melding = string.Format("Lag med id = '{0}' fantes ikke.", id);
            var httpError = new HttpError(melding);

            return Request.CreateResponse(HttpStatusCode.NotFound, httpError);
        }

        [NonAction]
        private HttpResponseMessage OpprettIngenLagFantesIkkeResponse()
        {
            var melding = string.Format("Ingen lag fantes");
            var httpError = new HttpError(melding);

            return Request.CreateResponse(HttpStatusCode.NotFound, httpError);
        }
    }
}