using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using BouvetCodeCamp.Dataaksess;
using BouvetCodeCamp.Dataaksess.Interfaces;
using BouvetCodeCamp.Dataaksess.Repositories;
using BouvetCodeCamp.Felles;
using BouvetCodeCamp.Felles.Interfaces;
using BouvetCodeCamp.Felles.Konfigurasjon;
using BouvetCodeCamp.Service.Interfaces;
using BouvetCodeCamp.Service.Services;
using Newtonsoft.Json.Serialization;
using Owin;
using Autofac;
using System.Reflection;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.SignalR;
using BouvetCodeCamp.SignalR;

namespace BouvetCodeCamp
{
    using Felles.Entiteter;

    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            
            Configure(config.Formatters, config);
            config.MapHttpAttributeRoutes();
            config.EnableSystemDiagnosticsTracing();
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<GameApi>().As<IGameApi>();
            builder.RegisterType<Konfigurasjon>().As<IKonfigurasjon>();
            builder.RegisterType<DocumentDbContext>().As<IDocumentDbContext>();

            builder.RegisterType<LagRepository>().As<Repository<Lag>>();

            builder.RegisterType<LagService>().As<ILagService>();
            builder.RegisterType<KodeService>().As<IKodeService>();

            builder.Register(x => GlobalHost.ConnectionManager.GetHubContext<IGameHub>("GameHub")).As<IHubContext<IGameHub>>();
            builder.RegisterType<CoordinateVerifier>().As<ICoordinateVerifier>();
            
            var container = builder.Build();
             // Create an assign a dependency resolver for Web API to use.
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // This should be the first middleware added to the IAppBuilder.
            appBuilder.UseAutofacMiddleware(container);

            // Make sure the Autofac lifetime scope is passed to Web API.
            appBuilder.UseAutofacWebApi(config);

            appBuilder.UseWebApi(config);

            var hubConfig = new HubConfiguration();
            hubConfig.EnableJSONP = true;
            appBuilder.MapSignalR(hubConfig);

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
