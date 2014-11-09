namespace Bouvet.BouvetBattleRoyale.Applikasjon.Owin
{
    using System;
    using System.Configuration;
    using System.Diagnostics.Eventing.Reader;
    using System.Globalization;
    using System.IO;
    using System.Net.Http.Formatting;
    using System.Reflection;
    using System.Threading;
    using System.Web.Http;
    using System.Web.Http.Cors;

    using Autofac;
    using Autofac.Integration.WebApi;

    using Bouvet.BouvetBattleRoyale.Applikasjon.Owin.Authentication;
    using Bouvet.BouvetBattleRoyale.Applikasjon.Owin.Filters;
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.CrossCutting;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Data;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Data.Interfaces;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Data.Repositories;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Interfaces;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Logging;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Worker.Queues;
    using Bouvet.BouvetBattleRoyale.Tjenester;
    using Bouvet.BouvetBattleRoyale.Tjenester.Interfaces;
    using Bouvet.BouvetBattleRoyale.Tjenester.Interfaces.Services;
    using Bouvet.BouvetBattleRoyale.Tjenester.Interfaces.SignalR.Hubs;
    using Bouvet.BouvetBattleRoyale.Tjenester.Services;
    using Bouvet.BouvetBattleRoyale.Tjenester.SignalR.Hubs;

    using BouvetCodeCamp.DomeneTjenester;

    using global::Owin;

    using log4net;

    using Microsoft.AspNet.SignalR;
    using Microsoft.Owin.Extensions;
    using Microsoft.WindowsAzure.ServiceRuntime;

    using Newtonsoft.Json.Serialization;

    using AutofacContrib.DynamicProxy;

    using Swashbuckle;
    using Swashbuckle.Application;
    using Bouvet.BouvetBattleRoyale.Applikasjon.Owin.Interception;

    public class Startup
    {
        private static ILog _log;

        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            Configure(config.Formatters, config);
            config.MapHttpAttributeRoutes();
            config.EnableSystemDiagnosticsTracing();
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            SetGlobalizationCulture("nb-NO");

            Log4NetLogger.InitialiserLogging<Startup>();
            _log = Log4NetLogger.HentLogger(typeof(Startup));

            appBuilder.Use(typeof(AuthenticationMiddleware));

#if RELEASE
            KonfigurerApiDokumentasjon(appBuilder, config);
#endif
            var builder = BuildContainer();

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
            
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
        }

        public static ContainerBuilder BuildContainer()
        {
            if (_log == null)
                _log = Log4NetLogger.HentLogger(typeof(Startup));

            var builder = new ContainerBuilder();
            builder.RegisterModule<log4netAutofacModule>();

            builder.RegisterType<RetryInterceptor>().AsSelf();
            // Note controller methods must be virtual to be intercepted
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).EnableClassInterceptors().InterceptedBy(typeof(RetryInterceptor));

            SettKonfigurasjon(builder);

            builder.RegisterType<DocumentDbContext>().As<IDocumentDbContext>().SingleInstance();

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

            builder.RegisterType<KoordinatVerifier>().As<IKoordinatVerifier>();

            // Infrastruktur
            builder.RegisterType<LagRepository>().As<IRepository<Lag>>().SingleInstance();
            builder.RegisterType<PostRepository>().As<IRepository<Post>>().SingleInstance();
            builder.RegisterType<GameStateRepository>().As<IRepository<GameState>>().SingleInstance();
            builder.RegisterType<LoggHendelseRepository>().As<IRepository<LoggHendelse>>().SingleInstance();

            builder.RegisterType<QueueArkivHandler>().As<IArkivHandler>();
            builder.RegisterType<QueueMessageConsumer>().As<IQueueMessageConsumer>();

            SettAktivMessageProducer(builder);

            builder.Register(x => GlobalHost.ConnectionManager.GetHubContext<IGameHub>("GameHub"))
                .As<IHubContext<IGameHub>>()
                .SingleInstance();

            builder.RegisterType<GameHubProxy>().As<IGameHub>();
            return builder;
        }

        private static void SettKonfigurasjon(ContainerBuilder builder)
        {
            if (RoleEnvironment.IsAvailable)
            {
                builder.RegisterType<RoleKonfigurasjon>().As<IKonfigurasjon>().SingleInstance(); // Cacher i instans
            }
            else
            {
                builder.RegisterType<Konfigurasjon>().As<IKonfigurasjon>().SingleInstance(); // Cacher i instans
            }
        }

        private static void SettAktivMessageProducer(ContainerBuilder builder)
        {
            const string AktivMessageProducerSettingKey = "AktivMessageProducer";

            var aktivMessageProducer = RoleEnvironment.IsAvailable
                                           ? RoleEnvironment.GetConfigurationSettingValue(AktivMessageProducerSettingKey)
                                           : ConfigurationManager.AppSettings[AktivMessageProducerSettingKey];

            switch (aktivMessageProducer)
            {
                case "QueueMessageProducer":
                    builder.RegisterType<QueueMessageProducer>().As<IQueueMessageProducer>();
                    _log.Info("Autofac->Aktiv MessageProducer er: QueueMessageProducer");
                    break;

                case "MemoryMessageProducer":
                    builder.RegisterType<MemoryMessageProducer>().As<IQueueMessageProducer>();
                    _log.Info("Autofac->Aktiv MessageProducer er: MemoryMessageProducer");
                    break;

                default:
                    throw new ConfigurationErrorsException("Ugyldig konfigurasjonsverdi for AktivMessageProducer: ukjent type");
            }
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var exception = unhandledExceptionEventArgs.ExceptionObject as Exception;

            if (exception != null)
                _log.Fatal("Uhåndtert feil: " + exception.Message, exception);
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
