namespace BouvetCodeCamp.Filters
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    public class HttpApiException : HttpResponseException
    {
        public HttpApiException(HttpResponseMessage message) : base(message) { }

        public HttpApiException(HttpStatusCode code) : base(code) { }
    }
}