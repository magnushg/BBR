namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Worker.Queues
{
    using System.Threading.Tasks;

    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Interfaces;

    using Microsoft.WindowsAzure.Storage.Queue;

    public class InMemoryMessageProducer : IQueueMessageProducer
    {
        private readonly IQueueMessageConsumer queueMessageConsumer;

        public InMemoryMessageProducer(IQueueMessageConsumer queueMessageConsumer)
        {
            this.queueMessageConsumer = queueMessageConsumer;
        }

        public async Task CreateMessage(LoggHendelse loggHendelse)
        {
            await queueMessageConsumer.KonsumerMelding(new CloudQueueMessage(string.Empty).Serialize(loggHendelse));
        }
    }
}