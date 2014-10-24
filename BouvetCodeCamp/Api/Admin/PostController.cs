namespace BouvetCodeCamp.Api.Admin
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Domene;
    using Domene.Entiteter;
    using DomeneTjenester.Interfaces;

    [RoutePrefix("api/admin/post")]
    [Authorize]
    public class PostController : BaseApiController
    {
        private readonly IService<Post> _postService;

        public PostController(IService<Post> postService)
        {
            _postService = postService;
        }

        // GET api/admin/post/get
        [Route("get")]
        [HttpGet]
        [Obsolete] // Skjule for Swagger-apidoc
        public HttpResponseMessage Get()
        {
            var poster = _postService.HentAlle();
            
            return Request.CreateResponse(HttpStatusCode.OK, poster);
        }

        // GET api/admin/post/a-b-c-d
        [Route("get/{id}")]
        [HttpGet]
        [Obsolete] // Skjule for Swagger-apidoc
        public HttpResponseMessage GetPost(string id)
        {
            if (string.IsNullOrEmpty(id))
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Mangler id");

            var post = _postService.Hent(id);

            if (post == null)
                return OpprettErrorResponse(
                    ErrorResponseType.FantIkkeObjekt,
                    string.Format("Post med id = '{0}' fantes ikke.", id));

            return Request.CreateResponse(HttpStatusCode.OK, post);
        }

        // POST api/admin/post/post
        [Route("post")]
        [HttpPost]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> PostPost([FromBody]Post modell)
        {
            if (modell == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig request");

            await _postService.Opprett(modell);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // PUT api/admin/post/put
        [Route("put")]
        [HttpPut]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> PutPost([FromBody]Post model)
        {
            if (model == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig request");

            await _postService.Oppdater(model);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/admin/post/delete
        [Route("delete")]
        [HttpDelete]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> Delete()
        {
            var allePoster = _postService.HentAlle();

            foreach (var post in allePoster)
            {
                await _postService.Slett(post);
            }
            
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/admin/post/delete/a-b-c-d
        [Route("delete/{id}")]
        [HttpDelete]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> DeletePost(string id)
        {
            if (string.IsNullOrEmpty(id))
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Mangler id");

            var post = _postService.Hent(id);

            if (post == null)
                return OpprettErrorResponse(
                    ErrorResponseType.FantIkkeObjekt,
                    string.Format("Post med id = '{0}' fantes ikke.", id));

            await _postService.Slett(post);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}