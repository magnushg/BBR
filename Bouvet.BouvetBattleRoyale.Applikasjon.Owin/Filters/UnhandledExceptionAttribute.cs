namespace BouvetCodeCamp.Filters
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Filters;

    using BouvetCodeCamp.CrossCutting;

    using log4net;

    public class UnhandledExceptionAttribute : ExceptionFilterAttribute
    {
       

        public UnhandledExceptionAttribute()
        {
         }

        public override void OnException(HttpActionExecutedContext context)
        {
            
            var response = context.Request.CreateErrorResponse(
                HttpStatusCode.InternalServerError, 
                context.Exception);

            context.Response = response;
        }
    }
}