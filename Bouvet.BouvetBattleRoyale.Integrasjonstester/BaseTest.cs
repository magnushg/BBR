namespace Bouvet.BouvetBattleRoyale.Integrasjonstester
{
    using Autofac;

    using Bouvet.BouvetBattleRoyale.Applikasjon.Owin;

    public abstract class BaseTest
    {
        protected BaseTest()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected T Resolve<T>() where T : class
        {
            var builder = Startup.BuildContainer();
            var container = builder.Build();
            return container.Resolve<T>();
        }
    }
}