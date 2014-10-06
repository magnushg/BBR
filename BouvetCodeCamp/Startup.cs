using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;

using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester;
using BouvetCodeCamp.DomeneTjenester.Interfaces;
using Newtonsoft.Json.Serialization;
using Owin;
using Autofac;
using System.Reflection;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.SignalR;
using BouvetCodeCamp.SignalR;

namespace BouvetCodeCamp
{
    using System;
    using System.IO;

    using BouvetCodeCamp.Authentication;
    using BouvetCodeCamp.Filters;

    using Infrastruktur.DataAksess;
    using Infrastruktur.DataAksess.Interfaces;
    using Infrastruktur.DataAksess.Repositories;

    using Microsoft.Owin.Extensions;

    using Swashbuckle.Application;

    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            Configure(config.Formatters, config);
            config.MapHttpAttributeRoutes();
            config.EnableSystemDiagnosticsTracing();
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            appBuilder.Use(typeof(AuthenticationMiddleware));

            KonfigurerApiDokumentasjon(appBuilder, config);
            
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<Konfigurasjon>().As<IKonfigurasjon>();
            builder.RegisterType<DocumentDbContext>().As<IDocumentDbContext>();

            // Services
            builder.RegisterType<LagService>().As<ILagService>();
            builder.RegisterType<PostService>().As<IPostService>();
            builder.RegisterType<DomeneTjenester.GameApi>().As<IGameApi>();
            builder.RegisterType<GameStateService>().As<IGameStateService>();

            // Repositories
            builder.RegisterType<LagRepository>().As<IRepository<Lag>>();
            builder.RegisterType<PostRepository>().As<IRepository<Post>>();
            builder.RegisterType<GameStateRepository>().As<IRepository<GameState>>();

            builder.RegisterType<KoordinatVerifier>().As<IKoordinatVerifier>();

<<<<<<< HEAD
            builder.Register(x => GlobalHost.ConnectionManager.GetHubContext<IGameHub>("GameHub")).As<IHubContext<IGameHub>>().SingleInstance();
            builder.RegisterType<CoordinateVerifier>().As<ICoordinateVerifier>();
            
=======
            builder.Register(x => GlobalHost.ConnectionManager.GetHubContext<IGameHub>("GameHub")).As<IHubContext<IGameHub>>();

>>>>>>> ee0771344500c4bc16203a6465b27b8b96283fea
            var container = builder.Build();
             // Create an assign a dependency resolver for Web API to use.
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // This should be the first middleware added to the IAppBuilder.
            appBuilder.UseAutofacMiddleware(container);

            // Make sure the Autofac lifetime scope is passed to Web API.
            appBuilder.UseAutofacWebApi(config);

            appBuilder.UseWebApi(config);
            
            var hubConfig = new HubConfiguration { EnableJSONP = true };

            appBuilder.MapSignalR(hubConfig);
        }

        private static void KonfigurerApiDokumentasjon(IAppBuilder appBuilder, HttpConfiguration config)
        {
            appBuilder.UseStageMarker(PipelineStage.MapHandler);

            Swashbuckle.Bootstrapper.Init(config);

            SwaggerSpecConfig.Customize(c =>
            {
                c.IgnoreObsoleteActions();
                c.IncludeXmlComments(GetXmlCommentsPath());
                c.OperationFilter<AddAuthorizationRequiredResponseCodes>();
            });
        }

        protected static string GetXmlCommentsPath()
        {
            try
            {
                return String.Format(@"{0}\docs\BouvetCodeCamp.XML", System.AppDomain.CurrentDomain.BaseDirectory);
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                throw new Exception("Fant ikke XML-dokumentasjon", fileNotFoundException);
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                throw new Exception("Fant ikke XML-dokumentasjon i mappen", directoryNotFoundException);
            }
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
