namespace BouvetCodeCamp.Integrasjonstester.DataAksess
{
    using System;
    using System.Configuration;

    using Autofac;

    using Bouvet.BouvetBattleRoyale.Applikasjon.Owin;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Data;

    using Microsoft.Azure.Documents.Client;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public abstract class BaseRepositoryIntegrasjonstest
    {
        private readonly string databaseId;
        private readonly string endpoint;
        private readonly string authKey;

        protected BaseRepositoryIntegrasjonstest()
        {
            this.databaseId = ConfigurationManager.AppSettings[DocumentDbKonstanter.DatabaseId];
            endpoint = ConfigurationManager.AppSettings[DocumentDbKonstanter.Endpoint];
            authKey = ConfigurationManager.AppSettings[DocumentDbKonstanter.AuthKey];
        }
        
        [TestInitialize]
        public void FørHverTest()
        {
            using (var client = new DocumentClient(new Uri(endpoint), authKey))
            {
                DocumentDbHelpers.SlettDatabaseAsync(client, this.databaseId);

                DocumentDbHelpers.HentEllerOpprettDatabaseAsync(client, databaseId);
            }
        }
        
        protected T Resolve<T>() where T : class
        {
            var builder = Startup.BuildContainer();
            var container = builder.Build();
            return container.Resolve<T>();
        }
    }
}