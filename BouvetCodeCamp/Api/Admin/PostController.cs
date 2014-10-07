namespace BouvetCodeCamp.Api.Admin
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using BouvetCodeCamp.Domene;
    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;

    [RoutePrefix("api/admin/post")]
    [Authorize]
    public class PostController : BaseApiController
    {
        private readonly IPostService postService;

        public PostController(IPostService postService)
        {
            this.postService = postService;
        }

        // GET api/admin/post/get
        [Route("get")]
        [HttpGet]
        [Obsolete] // Skjule for Swagger-apidoc
        public HttpResponseMessage Get()
        {
            var poster = postService.HentAlle();
            
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

            var post = postService.Hent(id);

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

            await postService.Opprett(modell);

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

            await postService.Oppdater(model);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/admin/post/delete
        [Route("delete")]
        [HttpDelete]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> Delete()
        {
            await postService.SlettAlle();
            
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
            
            var post = postService.Hent(id);

            if (post == null)
                return OpprettErrorResponse(
                    ErrorResponseType.FantIkkeObjekt,
                    string.Format("Post med id = '{0}' fantes ikke.", id));
            
            await postService.Slett(post);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}