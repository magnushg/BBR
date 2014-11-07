namespace Bouvet.BouvetBattleRoyale.Integrasjonstester.Infrastructure
{
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Worker.Queues;

    using FizzWare.NBuilder;

    using Microsoft.WindowsAzure.Storage.Queue;

    using NUnit.Framework;

    using Should;

    [TestFixture]
    public class CloudQueueMessageExtensionsTests
    {
        [Test]
        public void Deserialize_SerialisertLoggHendelse_DeserialisertObjektErLoggHendelse()
        {
            // Arrange
            var loggHendelse = Builder<LoggHendelse>.CreateNew().Build();

            // Act
            var serialisertLoggHendelse = new CloudQueueMessage(string.Empty).Serialize(loggHendelse);

            var deserialisertLoggHendelse = serialisertLoggHendelse.Deserialize<LoggHendelse>();

            // Assert
            deserialisertLoggHendelse.GetType().ShouldEqual(loggHendelse.GetType());
        }

        [Test]
        public void GetType_TypeErLoggHendelse_GirLoggHendelse()
        {
            // Arrange
            var loggHendelse = Builder<LoggHendelse>.CreateNew().Build();
            const string loggHendelseKlasseNavn = "LoggHendelse";

            // Act
            var serialisertLoggHendelse = new CloudQueueMessage(string.Empty).Serialize(loggHendelse);

            var meldingsTypeNavn = serialisertLoggHendelse.GetMessageTypeName();

            // Assert
            meldingsTypeNavn.ShouldEqual(loggHendelseKlasseNavn);
        } 
    }
}