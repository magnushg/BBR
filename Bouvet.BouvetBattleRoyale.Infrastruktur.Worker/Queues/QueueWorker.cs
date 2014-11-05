namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Worker.Queues
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;

    using log4net;

    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Queue;

    /// <summary>
    /// Details: http://alexandrebrisebois.wordpress.com/2013/02/19/windows-azure-queue-storage-service-polling-task/
    /// </summary>
    public abstract class QueueWorker : PollingTask<CloudQueueMessage>
    {
        private readonly CloudQueue poisonQueue;
        private readonly CloudQueue queue;
        private readonly TimeSpan visibilityTimeout;
        private readonly int maxAttempts;

        protected QueueWorker(ILog log)
            : base(log)
        {
            var queueName = ConfigurationManager.AppSettings["QueueName"];
            var poisonQueueName = ConfigurationManager.AppSettings["PoisonQueueName"];

            maxAttempts = int.Parse(ConfigurationManager.AppSettings["DequeueMaxAttempts"]);
            var visibilityTimeoutInMinutes = int.Parse(ConfigurationManager.AppSettings["DequeuedMessageVisibilityTimeoutInMinutes"]);

            var connectionStringFromConfig =
               ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString;

            var account = CloudStorageAccount.Parse(connectionStringFromConfig);

            CloudQueueClient client = account.CreateCloudQueueClient();

            ServicePointManager.FindServicePoint(account.QueueEndpoint).UseNagleAlgorithm = false;

            queue = client.GetQueueReference(queueName);
            queue.CreateIfNotExists();

            poisonQueue = client.GetQueueReference(poisonQueueName);
            poisonQueue.CreateIfNotExists();

            visibilityTimeout = TimeSpan.FromMinutes(visibilityTimeoutInMinutes);
        }

        protected override void Execute(CloudQueueMessage workItem)
        {
            if (workItem == null)
                throw new ArgumentNullException("workItem");

            if (workItem.DequeueCount > maxAttempts)
            {
                PlaceMessageOnPoisonQueue(workItem);
                return;
            }

            OnExecuting(workItem);
        }

        protected abstract void OnExecuting(CloudQueueMessage workItem);

        private void PlaceMessageOnPoisonQueue(CloudQueueMessage workItem)
        {
            var message = new CloudQueueMessage(workItem.AsString);
            poisonQueue.AddMessage(message);
            Completed(workItem);
        }

        protected override void Completed(CloudQueueMessage workItem)
        {
            try
            {
                queue.DeleteMessage(workItem);
            }
            catch (StorageException ex)
            {
                Report(ex.ToString());
            }
        }

        protected override ICollection<CloudQueueMessage> TryGetWork()
        {
            return queue.GetMessages(32, visibilityTimeout).ToList();
        }
    }
}