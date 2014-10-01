namespace BouvetCodeCamp.AdminApi
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using BouvetCodeCamp.GameApi;

    using Domene.Entiteter;
    using DomeneTjenester.Interfaces;

    [RoutePrefix("api/post")]
    [Authorize]
    public class PostController : BaseApiController
    {
        private readonly IPostService postService;

        public PostController(IPostService postService)
        {
            this.postService = postService;
        }

        // GET api/post/get
        [Route("get")]
        [HttpGet]
        [Obsolete] // Skjule for Swagger-apidoc
        public HttpResponseMessage Get()
        {
            var poster = postService.HentAlle();
            
            return this.Request.CreateResponse(HttpStatusCode.OK, poster);
        }

        // GET api/post/get/a-b-c-d
        [Route("get/{id}")]
        [HttpGet]
        [Obsolete] // Skjule for Swagger-apidoc
        public HttpResponseMessage GetPost(string id)
        {
            if (string.IsNullOrEmpty(id))
                return this.UgyldigRequestResponse("id");

            var post = postService.Hent(id);
            
            if (post == null)
            {
                return this.OpprettPostFantesIkkeResponse(id);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, post);
        }

        // POST api/post/post
        [Route("post")]
        [HttpPost]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> PostPost([FromBody]Post modell)
        {
            if (modell == null)
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig request");

            await this.postService.Opprett(modell);

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        // PUT api/post/put
        [Route("put")]
        [HttpPut]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> PutPost([FromBody]Post model)
        {
            if (model == null)
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig request");

            await this.postService.Oppdater(model);

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/post/delete
        [Route("delete")]
        [HttpDelete]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> Delete()
        {
            await postService.SlettAlle();
            
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/post/delete/a-b-c-d
        [Route("delete/{id}")]
        [HttpDelete]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> DeletePost(string id)
        {
            if (string.IsNullOrEmpty(id))
                return this.UgyldigRequestResponse("id");
            
            var post = postService.Hent(id);

            if (post == null)
                return this.OpprettPostFantesIkkeResponse(id);
            
            await postService.Slett(post);

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}