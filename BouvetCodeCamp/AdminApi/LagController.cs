namespace BouvetCodeCamp.AdminApi
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;

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
            var lagene = this.lagRepository.HentAlle();

            if (lagene == null || !lagene.Any()) 
                return this.OpprettIngenLagFantesIkkeResponse();

            return this.Request.CreateResponse(HttpStatusCode.OK, lagene);
        }

        // GET api/lag/get/a-b-c-d
        [Route("get/{id}")]
        [HttpGet]
        public HttpResponseMessage GetLag(string id)
        {
            var lag = this.lagRepository.Hent(id);

            if (lag == null)
            {
                return this.OpprettLagFantesIkkeResponse(id);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, lag);
        }

        // POST api/lag/post
        [Route("post")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostLag([FromBody]Lag model)
        {
            if (model == null) 
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig request");

            await this.lagRepository.Opprett(model);

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        // PUT api/lag/put
        [Route("put")]
        [HttpPut]
        public async Task<HttpResponseMessage> PutLag([FromBody]Lag model)
        {
            if (model == null)
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig request");

            await this.lagRepository.Oppdater(model);

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/lag/delete
        [Route("delete")]
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete()
        {
            await this.lagRepository.SlettAlle();
            
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/lag/delete/a-b-c-d
        [Route("delete/{id}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteLag(string id)
        {
            var lag = this.lagRepository.Hent(id);

            if (lag == null) 
                return this.OpprettLagFantesIkkeResponse(id);

            await this.lagRepository.Slett(lag);

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        [NonAction]
        private HttpResponseMessage OpprettLagFantesIkkeResponse(string id)
        {
            var melding = string.Format("Lag med id = '{0}' fantes ikke.", id);
            var httpError = new HttpError(melding);

            return this.Request.CreateResponse(HttpStatusCode.NotFound, httpError);
        }

        [NonAction]
        private HttpResponseMessage OpprettIngenLagFantesIkkeResponse()
        {
            var melding = string.Format("Ingen lag fantes");
            var httpError = new HttpError(melding);

            return this.Request.CreateResponse(HttpStatusCode.NotFound, httpError);
        }
    }
}