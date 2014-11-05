namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Worker.Queues
{
    using System;
    using System.Threading.Tasks;

    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Interfaces;

    using log4net;

    using Microsoft.WindowsAzure.Storage.Queue;

    public class QueueMessageConsumer : IQueueMessageConsumer
    {
        private readonly ILog log;

        private readonly IRepository<LoggHendelse> loggHendelseRepository;

        public QueueMessageConsumer(
            ILog log,
            IRepository<LoggHendelse> loggHendelseRepository)
        {
            this.log = log;
            this.loggHendelseRepository = loggHendelseRepository;
        }

        public async Task KonsumerMelding(CloudQueueMessage melding)
        {
            log.Info("Konsumer melding: " + melding.AsString);

            var type = melding.GetMessageType();

            if (type == typeof(LoggHendelse))
            {
                try
                {
                    var loggHendelse = melding.Deserialize<LoggHendelse>();

                    await loggHendelseRepository.Opprett(loggHendelse);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                log.Warn("Ukjent objekttype i melding");
            }
        }
    }
}