namespace BouvetCodeCamp.Integrasjonstester.DataAksess
{
    using System;
    using System.Configuration;

    using BouvetCodeCamp.Dataaksess;
    using BouvetCodeCamp.Felles;

    using Microsoft.Azure.Documents.Client;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public abstract class BaseRepositoryIntegrasjonstest
    {
        private readonly string databaseId;

        protected BaseRepositoryIntegrasjonstest()
        {
            this.databaseId = ConfigurationManager.AppSettings[DocumentDbKonstanter.DatabaseId];
        }
        
        [TestInitialize]
        public async void FørHverTest()
        {
            DocumentClient client;

            using (client = new DocumentClient(new Uri(DocumentDbKonstanter.Endpoint), DocumentDbKonstanter.AuthKey))
            {
                await DocumentDbHelpers.HentEllerOpprettDatabaseAsync(client, this.databaseId);
            }
        }

        [TestCleanup]
        public async void EtterHverTest()
        {
            DocumentClient client;

            using (client = new DocumentClient(new Uri(DocumentDbKonstanter.Endpoint), DocumentDbKonstanter.AuthKey))
            {
                await DocumentDbHelpers.SlettDatabaseAsync(client, this.databaseId);
            }
        }
    }
}