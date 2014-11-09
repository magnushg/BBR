namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Azure.WorkerRole
{
    using Autofac;

    using Bouvet.BouvetBattleRoyale.Applikasjon.Owin;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.CrossCutting;

    public static class AutofacContainerBuilder
    {
        public static IContainer BuildAutofacContainer()
        {
            var builder = Startup.BuildContainer();
            
            builder.RegisterType<RoleKonfigurasjon>().As<IKonfigurasjon>();

            return builder.Build();
        }
    }
}