using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;

using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester;
using BouvetCodeCamp.DomeneTjenester.Interfaces;
using BouvetCodeCamp.DomeneTjenester.Services;
using BouvetCodeCamp.SignalR.Hubs;
using Newtonsoft.Json.Serialization;
using Owin;
using Autofac;
using System.Reflection;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.SignalR;
using Swashbuckle;

namespace BouvetCodeCamp
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading;

    using Authentication;

    using BouvetCodeCamp.CrossCutting;

    using Filters;

    using Infrastruktur.DataAksess;
    using Infrastruktur.DataAksess.Interfaces;
    using Infrastruktur.DataAksess.Repositories;

    using log4net;

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
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;            
            SetGlobalizationCulture("nb-NO");

            appBuilder.Use(typeof(AuthenticationMiddleware));

            KonfigurerApiDokumentasjon(appBuilder, config);

            var builder = new ContainerBuilder();
            builder.RegisterModule<log4netAutofacModule>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<Konfigurasjon>().As<IKonfigurasjon>();
            builder.RegisterType<DocumentDbContext>().As<IDocumentDbContext>();

            //singleton memory instance
            builder.RegisterType<GameState>().SingleInstance();

            // Services
            builder.RegisterType<LagService>().As<IService<Lag>>();
            builder.RegisterType<LagGameService>().As<ILagGameService>();
            builder.RegisterType<PostService>().As<IService<Post>>();
            builder.RegisterType<PostGameService>().As<IPostGameService>();
            builder.RegisterType<GameStateService>().As<IService<GameState>>();
            builder.RegisterType<GameApi>().As<IGameApi>();
            builder.RegisterType<GameStateService>().As<IService<GameState>>();
            builder.RegisterType<PoengService>().As<IPoengService>();

            // Repositories
            builder.RegisterType<LagRepository>().As<IRepository<Lag>>();
            builder.RegisterType<PostRepository>().As<IRepository<Post>>();
            builder.RegisterType<GameStateRepository>().As<IRepository<GameState>>();

            builder.RegisterType<KoordinatVerifier>().As<IKoordinatVerifier>();

            builder.Register(x => GlobalHost.ConnectionManager.GetHubContext<IGameHub>("GameHub")).As<IHubContext<IGameHub>>().SingleInstance();
            builder.RegisterType<GameHubProxy>().As<IGameHub>();            

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

            InitialiserLogging();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var log = Log4NetLogger.HentLogger(typeof(Startup));
            var exception = unhandledExceptionEventArgs.ExceptionObject as Exception;

            if (exception != null)
                log.Fatal("Uhåndtert feil: " + exception.Message, exception);
        }

        private static void InitialiserLogging()
        {
            log4net.Config.XmlConfigurator.Configure();

            var log = LogManager.GetLogger(typeof(Startup));

            log.Info("Startup ok.");
        }

        private void SetGlobalizationCulture(string cultureLanguage)
        {
            var culture = CultureInfo.CreateSpecificCulture(cultureLanguage);

            Thread.CurrentThread.CurrentCulture = culture;
        }

        private static void KonfigurerApiDokumentasjon(IAppBuilder appBuilder, HttpConfiguration config)
        {
            appBuilder.UseStageMarker(PipelineStage.MapHandler);

            Bootstrapper.Init(config);

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
                return String.Format(@"{0}\docs\BouvetCodeCamp.XML", AppDomain.CurrentDomain.BaseDirectory);
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
