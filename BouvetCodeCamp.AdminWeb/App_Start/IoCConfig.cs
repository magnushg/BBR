namespace BouvetCodeCamp.AdminWeb.App_Start
{
    using System.Web.Mvc;

    using Autofac;
    using Autofac.Integration.Mvc;

    using BouvetCodeCamp.Dataaksess;
    using BouvetCodeCamp.Dataaksess.Interfaces;
    using BouvetCodeCamp.Dataaksess.Repositories;
    using BouvetCodeCamp.Felles;
    using BouvetCodeCamp.Felles.Entiteter;
    using BouvetCodeCamp.Felles.Interfaces;
    using BouvetCodeCamp.Felles.Konfigurasjon;

    public class IoCConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly).InstancePerRequest();

            builder.RegisterAssemblyTypes()
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterType<GameApi>().As<IGameApi>();
            builder.RegisterType<Konfigurasjon>().As<IKonfigurasjon>();
            builder.RegisterType<DocumentDbContext>().As<IDocumentDbContext>();

            builder.RegisterType<CoordinateVerifier>().As<ICoordinateVerifier>();

            builder.RegisterType<LagRepository>().As<IRepository<Lag>>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}