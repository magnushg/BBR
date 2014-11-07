namespace Bouvet.BouvetBattleRoyale.Integrasjonstester
{
    using Autofac;

    using Bouvet.BouvetBattleRoyale.Applikasjon.Owin;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Logging;

    public abstract class BaseTest
    {
        protected BaseTest()
        {
            Log4NetLogger.InitialiserLogging<BaseTest>();
        }

        protected T Resolve<T>() where T : class
        {
            var builder = Startup.BuildContainer();
            var container = builder.Build();
            return container.Resolve<T>();
        }
    }
}