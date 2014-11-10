namespace Bouvet.BouvetBattleRoyale.Integrasjonstester.Infrastruktur.Worker.Queues
{
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Interfaces;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Worker.Queues;

    using FizzWare.NBuilder;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class QueueArkivHandlerTests : BaseTest
    {
        private readonly Mock<IQueueMessageProducer> _queueMessageProducerMock = new Mock<IQueueMessageProducer>();

        private IArkivHandler _arkivHandler;

        [SetUp]
        public void SetUp()
        {
            _arkivHandler = new QueueArkivHandler(_queueMessageProducerMock.Object);
        }

        [Test]
        public void SendTilArkivet_EnLoggHendelse_LoggHendelsenErSendtTilQueueMessageProducer()
        {
            // Arrange
            var loggHendelse = Builder<LoggHendelse>.CreateNew().Build();

            // Act
            _arkivHandler.SendTilArkivet(loggHendelse);

            // Assert
            _queueMessageProducerMock.Verify(o => o.CreateMessage(It.IsAny<LoggHendelse>()), Times.Once());
        }
    }
}