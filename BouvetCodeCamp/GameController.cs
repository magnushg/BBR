using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiHost
{
   public class GameController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                value = "Welcome to Bouvet Code Camp"
            });
        }
    } 
}
