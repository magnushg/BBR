namespace BouvetCodeCamp.Lasttesting
{
    using System.Web.Http;

    public class BouvetCodeCampApiApplication : IApiApplication
    {
        private readonly HttpConfiguration _configuration;

        public BouvetCodeCampApiApplication(HttpConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Start()
        {
            _configuration.Routes.MapHttpRoute(
                name: "API Default",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}