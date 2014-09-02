using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Web.Http;
using System.Web.Http.Routing.Constraints;
using System.Web.Routing;

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

       public HttpResponseMessage ReportPosition(int groupId, double lat, double lon)
       {
            return new HttpResponseMessage();    
       }

        [HttpGet]
        [Route("api/game/sendCommand")]
        public HttpResponseMessage SendCommand(int groupId, Direction direction, double distance, string message)
        {
           return Request.CreateResponse(HttpStatusCode.OK, new
           {
                message = string.Format("You chose to move {0} for {1} meters with message {2}", direction, distance, message)
           });
        }


    }

    public enum Direction
    {
        North,
        East,
        West,
        South
    }
}
