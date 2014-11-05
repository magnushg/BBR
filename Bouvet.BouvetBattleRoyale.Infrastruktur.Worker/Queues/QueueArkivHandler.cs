namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Worker.Queues
{
    using System.Threading.Tasks;

    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Interfaces;

    public class QueueArkivHandler : IArkivHandler
    {
        private readonly IQueueMessageProducer queueMessageProducer;

        public QueueArkivHandler(IQueueMessageProducer queueMessageProducer)
        {
            this.queueMessageProducer = queueMessageProducer;
        }

        public async Task SendTilArkivet<T>(T entitet)
        {
            if (entitet is LoggHendelse)
                await queueMessageProducer.CreateMessage(entitet as LoggHendelse);
        }
    }
}