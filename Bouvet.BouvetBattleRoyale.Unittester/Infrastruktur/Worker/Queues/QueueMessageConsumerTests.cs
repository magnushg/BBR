namespace Bouvet.BouvetBattleRoyale.Unittests.Infrastruktur.Worker.Queues
{
    using System;
    using System.Threading.Tasks;

    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Interfaces;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Worker.Queues;

    using log4net;

    using Microsoft.WindowsAzure.Storage.Queue;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class QueueMessageConsumerTests
    {
        private readonly Mock<ILog> _logMock = new Mock<ILog>();
        private readonly Mock<IRepository<LoggHendelse>> _loggHendelseRepositoryMock = new Mock<IRepository<LoggHendelse>>();

        private QueueMessageConsumer queueMessageConsumer;

        [SetUp]
        public void Setup()
        {
            queueMessageConsumer = new QueueMessageConsumer(_logMock.Object, _loggHendelseRepositoryMock.Object);
        }

        [Test]
        public async Task KonsumerMelding_MeldingInneholderEnLoggHendelse_BrukerOpprettILoggHendelseRepository()
        {
            // Arrange
            const string Melding = "Bouvet.BouvetBattleRoyale.Domene.Entiteter.LoggHendelse:{\"hendelsesType\":2,\"tid\":\"2014-11-05T22:36:13.4162646+01:00\",\"kommentar\":\"1000 poeng for post 1\",\"lagId\":\"testlag1\",\"id\":\"\",\"_self\":\"\",\"_etag\":\"\"}";
            var cloudQueueMessage = new CloudQueueMessage(Melding);

            // Act
            await queueMessageConsumer.KonsumerMelding(cloudQueueMessage);

            // Assert
            _loggHendelseRepositoryMock.Verify(o => o.Opprett(It.IsAny<LoggHendelse>()), Times.Once);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public async Task KonsumerMelding_MeldingInneholderUkjentType_KasterException()
        {
            // Arrange
            const string Melding = "Bouvet.BouvetBattleRoyale.Domene.Entiteter.Lag:{\"hendelsesType\":2,\"tid\":\"2014-11-05T22:36:13.4162646+01:00\",\"kommentar\":\"1000 poeng for post 1\",\"lagId\":\"testlag1\",\"id\":\"\",\"_self\":\"\",\"_etag\":\"\"}";
            var cloudQueueMessage = new CloudQueueMessage(Melding);

            // Act
            await queueMessageConsumer.KonsumerMelding(cloudQueueMessage);
        }
    }
}