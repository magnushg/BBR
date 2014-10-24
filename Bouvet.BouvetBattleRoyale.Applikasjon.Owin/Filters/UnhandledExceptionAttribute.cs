namespace Bouvet.BouvetBattleRoyale.Applikasjon.Owin.Filters
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Filters;

    public class UnhandledExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {

            var response = context.Request.CreateErrorResponse(
                HttpStatusCode.InternalServerError,
                context.Exception);

            context.Response = response;
        }
    }
}