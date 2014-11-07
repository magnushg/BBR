namespace Bouvet.BouvetBattleRoyale.Integrasjonstester.Infrastructure.Worker
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Bouvet.BouvetBattleRoyale.Applikasjon.Owin;
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Data;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Interfaces;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Logging;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Worker.Queues;

    using FizzWare.NBuilder;

    using Microsoft.Azure.Documents.Client;

    using NUnit.Framework;

    using Should;

    [TestFixture]
    public class QueueMessageProducerTests : BaseTest
    {
        private string databaseId;
        private string endpoint;
        private string authKey;

        protected const string TestLagId = "testlag1";
        
        [SetUp]
        public void FørHverTest()
        {
            Log4NetLogger.InitialiserLogging<BaseTest>();

            TømDatabasen();

            var queueMessageConsumer = Resolve<IQueueMessageConsumer>();

            var log = Log4NetLogger.HentLogger(typeof(QueueMessageProducerTests));

            var worker = new MessageQueueWorker(queueMessageConsumer, log);

            worker.Start();
        }

        private void TømDatabasen()
        {
            databaseId = ConfigurationManager.AppSettings[DocumentDbKonstanter.DatabaseId];
            endpoint = ConfigurationManager.AppSettings[DocumentDbKonstanter.Endpoint];
            authKey = ConfigurationManager.AppSettings[DocumentDbKonstanter.AuthKey];

            using (var client = new DocumentClient(new Uri(endpoint), authKey))
            {
                DocumentDbHelpers.SlettDatabaseAsync(client, databaseId);

                DocumentDbHelpers.HentEllerOpprettDatabaseAsync(client, databaseId);
            }
        }

        [Test]
        public async Task CreateMessage_MeldingInneholderEnLoggHendelse_LoggHendelsenErLagret()
        {
            // Arrange
            var loggHendelse = Builder<LoggHendelse>.CreateNew()
                .With(o => o.LagId = TestLagId)
                .Build();
            
            var queueMessageProducer = Resolve<IQueueMessageProducer>();
            
            var repository = Resolve<IRepository<LoggHendelse>>();
            
            // Act
            await queueMessageProducer.CreateMessage(loggHendelse);
            
            Thread.Sleep(10000); // Vente til meldingen er ferdig behandlet og lagret i db
            
            // Assert
            var alle = repository.HentAlle();

            alle.ShouldNotBeEmpty();
            alle.Count().ShouldEqual(1);
        }
    }
}