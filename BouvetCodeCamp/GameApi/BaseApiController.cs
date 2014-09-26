namespace BouvetCodeCamp.GameApi
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using BouvetCodeCamp.Domene;

    public class BaseApiController : ApiController
    {
        public HttpResponseMessage OpprettErrorResponse(ErrorResponseType errorResponseType)
        {
            switch (errorResponseType)
            {
                case ErrorResponseType.UgyldigInputFormat:
                    return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig inputformat");
            }
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig forespørsel");
        }
    }
}