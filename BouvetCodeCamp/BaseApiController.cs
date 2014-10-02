namespace BouvetCodeCamp
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using BouvetCodeCamp.Domene;

    public class BaseApiController : ApiController
    {
        [NonAction]
        protected HttpResponseMessage OpprettErrorResponse(ErrorResponseType errorResponseType, string feilbeskrivelse)
        {
            switch (errorResponseType)
            {
                case ErrorResponseType.FantIkkeObjekt:
                    return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Fant ikke objekt: " + feilbeskrivelse);
                case ErrorResponseType.UgyldigInputFormat:
                    return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig request: " + feilbeskrivelse);
            }
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig forespørsel");
        }
    }
}