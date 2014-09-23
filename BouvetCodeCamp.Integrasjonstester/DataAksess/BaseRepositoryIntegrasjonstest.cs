namespace BouvetCodeCamp.Integrasjonstester.DataAksess
{
    using System;
    using System.Configuration;

    using BouvetCodeCamp.Dataaksess;
    using Felles;

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
        public async void FørHverTest()
        {
            using (var client = new DocumentClient(new Uri(endpoint), authKey))
            {
                await DocumentDbHelpers.SlettDatabaseAsync(client, this.databaseId);

                await DocumentDbHelpers.HentEllerOpprettDatabaseAsync(client, databaseId);
            }
        }
    }
}