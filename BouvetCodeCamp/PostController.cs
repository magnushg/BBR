namespace BouvetCodeCamp
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using BouvetCodeCamp.Dataaksess.Interfaces;
    using BouvetCodeCamp.Felles.Entiteter;

    [RoutePrefix("api/post")]
    public class PostController : ApiController
    {
        private readonly IRepository<Post> postRepository;

        public PostController(IRepository<Post> postRepository)
        {
            this.postRepository = postRepository;
        }

        // GET api/post/get
        [Route("get")]
        [HttpGet]
        public async Task<HttpResponseMessage> Get()
        {
            var poster = await postRepository.HentAlle();

            if (poster == null || !poster.Any())
                return this.OpprettIngenPosterFantesIkkeResponse();

            return Request.CreateResponse(HttpStatusCode.OK, poster);
        }

        // GET api/post/get/a-b-c-d
        [Route("get/{id}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetPost(string id)
        {
            var post = await postRepository.Hent(id);

            if (post == null)
            {
                return this.OpprettPostFantesIkkeResponse(id);
            }

            return Request.CreateResponse(HttpStatusCode.OK, post);
        }

        // POST api/post/post
        [Route("post")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostPost([FromBody]Post model)
        {
            if (model == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig request");

            await postRepository.Opprett(model);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // PUT api/post/put
        [Route("put")]
        [HttpPut]
        public async Task<HttpResponseMessage> PutPost([FromBody]Post model)
        {
            if (model == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig request");

            await postRepository.Oppdater(model);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/post/delete/a-b-c-d
        [Route("delete/{id}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeletePost(string id)
        {
            var post = await postRepository.Hent(id);

            if (post == null)
                return this.OpprettPostFantesIkkeResponse(id);

            await postRepository.Slett(post);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [NonAction]
        private HttpResponseMessage OpprettPostFantesIkkeResponse(string id)
        {
            var melding = string.Format("Post med id = '{0}' fantes ikke.", id);
            var httpError = new HttpError(melding);

            return Request.CreateResponse(HttpStatusCode.NotFound, httpError);
        }

        [NonAction]
        private HttpResponseMessage OpprettIngenPosterFantesIkkeResponse()
        {
            var melding = string.Format("Ingen poster fantes");
            var httpError = new HttpError(melding);

            return Request.CreateResponse(HttpStatusCode.NotFound, httpError);
        }
    }
}