namespace Bouvet.BouvetBattleRoyale.Unittests.Infrastruktur.Worker.Queues
{
    using System;

    using Bouvet.BouvetBattleRoyale.Domene;
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
        public void Serialize_SerialiserLoggHendelse_GirLoggHendelseSomJsonString()
        {
            // Arrange 
            const string Melding = "Bouvet.BouvetBattleRoyale.Domene.Entiteter.LoggHendelse:{\"hendelsesType\":2,\"tid\":\"2014-11-05T22:36:13.4162646+01:00\",\"kommentar\":\"1000 poeng for post 1\",\"lagId\":\"testlag1\",\"id\":\"\",\"_self\":\"\",\"_etag\":\"\"}";
            var cloudQueueMessage = new CloudQueueMessage(Melding);

            var loggHendelse = Builder<LoggHendelse>.CreateNew()
                .With(o => o.HendelseType = HendelseType.RegistrertKodeSuksess)
                .With(o => o.Tid = DateTime.Parse("2014-11-05T22:36:13.4162646+01:00"))
                .With(o => o.LagId = "testlag1")
                .With(o => o.Kommentar = "1000 poeng for post 1")
                .Build();

            // Act
            var resultat = cloudQueueMessage.Serialize(loggHendelse);

            // Assert
            resultat.AsString.ShouldEqual(Melding);
        }

        [Test]
        public void GetMessageTypeName_CloudQueueMessageInneholderLoggHendelse_GirLoggHendelse()
        {
            // Arrange 
            const string Melding = "Bouvet.BouvetBattleRoyale.Domene.Entiteter.LoggHendelse:{\"hendelsesType\":2,\"tid\":\"2014-11-05T22:36:13.4162646+01:00\",\"kommentar\":\"1000 poeng for post 1\",\"lagId\":\"testlag1\",\"id\":\"\",\"_self\":\"\",\"_etag\":\"\"}";
            var cloudQueueMessage = new CloudQueueMessage(Melding);
            
            // Act
            var resultat = cloudQueueMessage.GetMessageTypeName();

            // Assert
            resultat.ShouldEqual("LoggHendelse");
        }

        [Test]
        public void Deserialize_SerialisertLoggHendelse_GirLoggHendelseObjekt()
        {
            // Arrange 
            const string Melding = "Bouvet.BouvetBattleRoyale.Domene.Entiteter.LoggHendelse:{\"hendelsesType\":2,\"tid\":\"2014-11-05T22:36:13.4162646+01:00\",\"kommentar\":\"1000 poeng for post 1\",\"lagId\":\"testlag1\",\"id\":\"\",\"_self\":\"\",\"_etag\":\"\"}";
            var cloudQueueMessage = new CloudQueueMessage(Melding);

            var loggHendelse = Builder<LoggHendelse>.CreateNew()
                .With(o => o.HendelseType = HendelseType.RegistrertKodeSuksess)
                .With(o => o.Tid = DateTime.Parse("2014-11-05T22:36:13.4162646+01:00"))
                .With(o => o.LagId = "testlag1")
                .With(o => o.Kommentar = "1000 poeng for post 1")
                .Build();

            // Act
            var resultat = cloudQueueMessage.Deserialize<LoggHendelse>();

            // Assert
            resultat.HendelseType.ShouldEqual(loggHendelse.HendelseType);
            resultat.Tid.ShouldEqual(loggHendelse.Tid);
            resultat.LagId.ShouldEqual(loggHendelse.LagId);
            resultat.Kommentar.ShouldEqual(loggHendelse.Kommentar);
        }
    }
}