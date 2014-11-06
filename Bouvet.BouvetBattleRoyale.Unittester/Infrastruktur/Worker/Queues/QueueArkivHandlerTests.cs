namespace Bouvet.BouvetBattleRoyale.Unittests.Infrastruktur.Worker.Queues
{
    using System;
    using System.Threading.Tasks;

    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Interfaces;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Worker.Queues;

    using FizzWare.NBuilder;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class QueueArkivHandlerTests
    {
        private readonly Mock<IQueueMessageProducer> _queueMessageProducerMock = new Mock<IQueueMessageProducer>();

        private QueueArkivHandler arkivHandler;

        [SetUp]
        public void Setup()
        {
            arkivHandler = new QueueArkivHandler(_queueMessageProducerMock.Object);
        }

        [Test]
        public async Task SendTilArkivet_LoggHendelse_BrukerQueueMessageProducer()
        {
            // Arrange
            var loggHendelse = Builder<LoggHendelse>.CreateNew().Build();

            // Act
            await arkivHandler.SendTilArkivet(loggHendelse);

            // Assert
            _queueMessageProducerMock.Verify(o => o.CreateMessage(It.IsAny<LoggHendelse>()), Times.Once);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public async Task SendTilArkivet_UkjentType_KasterException()
        {
            // Arrange
            var arkivHandler = new QueueArkivHandler(_queueMessageProducerMock.Object);

            var lag = Builder<Lag>.CreateNew().Build();

            // Act
            await arkivHandler.SendTilArkivet(lag);
        }
    }
}