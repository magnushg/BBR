using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json.Serialization;
using Owin;

namespace BouvetCodeCamp
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();
            
            Configure(config.Formatters, config);
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );
            config.EnableSystemDiagnosticsTracing();
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            appBuilder.UseWebApi(config);
        }

        private static void Configure(MediaTypeFormatterCollection formatters, HttpConfiguration config)
        {
            var xml = config.Formatters.XmlFormatter;
            formatters.Remove(xml);

            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
