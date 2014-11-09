namespace Bouvet.BouvetBattleRoyale.Integrasjonstester.DataAksess
{
    using System;
    using System.Configuration;

    using Autofac;

    using Bouvet.BouvetBattleRoyale.Applikasjon.Owin;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.CrossCutting;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Data;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Logging;

    using Microsoft.Azure.Documents.Client;

    using NUnit.Framework;

    [TestFixture]
    public abstract class BaseRepositoryIntegrasjonstest
    {
        private readonly string databaseId;
        private readonly string endpoint;
        private readonly string authKey;
        
        protected const string TestLagId = "testlag1";

        protected BaseRepositoryIntegrasjonstest()
        {
            var konfigurasjon = Resolve<IKonfigurasjon>();

            databaseId = konfigurasjon.HentAppSetting(DocumentDbKonstanter.DatabaseId);
            endpoint = konfigurasjon.HentAppSetting(DocumentDbKonstanter.Endpoint);
            authKey = konfigurasjon.HentAppSetting(DocumentDbKonstanter.AuthKey);
        }

        [SetUp]
        public void FørHverTest()
        {
            Log4NetLogger.InitialiserLogging<BaseRepositoryIntegrasjonstest>();

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