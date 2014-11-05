namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Interfaces
{
    using System.Threading.Tasks;

    using Microsoft.WindowsAzure.Storage.Queue;

    public interface IQueueMessageConsumer
    {
        Task KonsumerMelding(CloudQueueMessage melding);
    }
}
