namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Azure.WorkerRole
{
    using Autofac;

    using Bouvet.BouvetBattleRoyale.Applikasjon.Owin;

    public static class AutofacContainerBuilder
    {
        public static IContainer BuildAutofacContainer()
        {
            var builder = Startup.BuildContainer();

            return builder.Build();
        }
    }
}