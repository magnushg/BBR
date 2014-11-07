namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Worker.Queues
{
    using System.Configuration;
    using System.Net;
    using System.Threading.Tasks;

    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Interfaces;

    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Queue;

    public class QueueMessageProducer : IQueueMessageProducer
    {
        private readonly CloudQueue queue;

        public QueueMessageProducer()
        {
            var queueName = ConfigurationManager.AppSettings["QueueName"];

            var connectionStringFromConfig =
                    ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString;

            var account = CloudStorageAccount.Parse(connectionStringFromConfig);

            CloudQueueClient client = account.CreateCloudQueueClient();

            ServicePointManager.FindServicePoint(account.QueueEndpoint).UseNagleAlgorithm = false;

            queue = client.GetQueueReference(queueName);

            queue.CreateIfNotExists();
        }

        public async Task CreateMessage(LoggHendelse loggHendelse)
        {
            await queue.AddMessageAsync(new CloudQueueMessage(string.Empty).Serialize(loggHendelse));
        }
    }
}