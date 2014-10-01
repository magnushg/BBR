namespace BouvetCodeCamp.GameApi
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using BouvetCodeCamp.Domene;

    public class BaseApiController : ApiController
    {
        [NonAction]
        protected HttpResponseMessage OpprettErrorResponse(ErrorResponseType errorResponseType)
        {
            switch (errorResponseType)
            {
                case ErrorResponseType.UgyldigInputFormat:
                    return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig inputformat");
            }
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig forespørsel");
        }
        
        [NonAction]
        protected HttpResponseMessage UgyldigRequestResponse(string felt)
        {
            var melding = string.Format("Request mangler: {0}", felt);
            var httpError = new HttpError(melding);

            return Request.CreateResponse(HttpStatusCode.NotFound, httpError);
        }

        [NonAction]
        protected HttpResponseMessage LagFantesIkkeResponse(string id)
        {
            var melding = string.Format("Lag med id = '{0}' fantes ikke.", id);
            var httpError = new HttpError(melding);

            return Request.CreateResponse(HttpStatusCode.NotFound, httpError);
        }

        [NonAction]
        protected HttpResponseMessage OpprettPostFantesIkkeResponse(string id)
        {
            var melding = string.Format("Post med id = '{0}' fantes ikke.", id);
            var httpError = new HttpError(melding);

            return this.Request.CreateResponse(HttpStatusCode.NotFound, httpError);
        }
    }
}