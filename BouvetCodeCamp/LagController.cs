namespace BouvetCodeCamp
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using BouvetCodeCamp.Dataaksess.Interfaces;
    using BouvetCodeCamp.Felles.Entiteter;
    
    [RoutePrefix("api/lag")]
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
        public async Task<HttpResponseMessage> Get()
        {
            var lagene = await lagRepository.HentAlle();

            if (lagene == null || !lagene.Any()) 
                return this.OpprettIngenLagFantesIkkeResponse();

            return Request.CreateResponse(HttpStatusCode.OK, lagene);
        }

        // GET api/lag/get/a-b-c-d
        [Route("get/{id}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetLag(string id)
        {
            var lag = await lagRepository.Hent(id);

            if (lag == null)
            {
                return this.OpprettLagFantesIkkeResponse(id);
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

        // DELETE api/lag/delete/a-b-c-d
        [Route("delete/{id}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteLag(string id)
        {
            var lag = await lagRepository.Hent(id);

            if (lag == null) 
                return this.OpprettLagFantesIkkeResponse(id);

            await lagRepository.Slett(lag);

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