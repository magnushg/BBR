namespace Bouvet.BouvetBattleRoyale.Applikasjon.Owin
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Bouvet.BouvetBattleRoyale.Applikasjon.Owin.Filters;
    using Bouvet.BouvetBattleRoyale.Domene;

    using BouvetCodeCamp.Domene;

    [UnhandledException]
    public class BaseApiController : ApiController
    {
        [NonAction]
        protected HttpResponseMessage OpprettErrorResponse(ErrorResponseType errorResponseType, string feilbeskrivelse)
        {
            switch (errorResponseType)
            {
                case ErrorResponseType.FantIkkeObjekt:
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Fant ikke objekt: " + feilbeskrivelse);
                case ErrorResponseType.UgyldigInputFormat:
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig request: " + feilbeskrivelse);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig forespørsel");
        }
    }
}