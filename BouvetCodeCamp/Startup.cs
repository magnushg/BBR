﻿using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using BouvetCodeCamp.Dataaksess;
using BouvetCodeCamp.Dataaksess.Interfaces;
using BouvetCodeCamp.Dataaksess.Repositories;
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
    using FakeItEasy;

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

            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<GameApi>().As<IGameApi>();
            builder.RegisterType<Konfigurasjon>().As<IKonfigurasjon>();
            builder.RegisterType<DocumentDbContext>().As<IDocumentDbContext>();

            // Services
            builder.RegisterType<LagService>().As<ILagService>();
            builder.RegisterType<KodeService>().As<IKodeService>();
            builder.RegisterType<FakeLoggSerive>().As<ILoggService>();

            // Repositories
            builder.RegisterType<LagRepository>().As<IRepository<Lag>>();
            builder.RegisterType<PostRepository>().As<IRepository<Post>>();
            builder.RegisterType<LoggRepository>().As<IRepository<LoggHendelse>>();

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
