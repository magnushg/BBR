namespace BouvetCodeCamp.Filters
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Filters;

    using BouvetCodeCamp.CrossCutting;

    using log4net;

    public class UnhandledExceptionAttribute : ExceptionFilterAttribute
    {
        private readonly ILog log;

        public UnhandledExceptionAttribute()
        {
            log = Log4NetLogger.HentLogger(typeof(UnhandledExceptionAttribute));
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            log.Error("API-feil: " + context.Exception.Message, context.Exception);
            
            var response = context.Request.CreateErrorResponse(
                HttpStatusCode.InternalServerError, 
                context.Exception);

            context.Response = response;
        }
    }
}