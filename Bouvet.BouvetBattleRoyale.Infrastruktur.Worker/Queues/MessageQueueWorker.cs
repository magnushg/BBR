namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Worker.Queues
{
    using System;
    using System.Threading;

    using Bouvet.BouvetBattleRoyale.Infrastruktur.CrossCutting;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Interfaces;

    using log4net;

    using Microsoft.WindowsAzure.Storage.Queue;

    public class MessageQueueWorker : QueueWorker
    {
        private readonly IQueueMessageConsumer queueMessageConsumer;

        private readonly ILog log;

        public MessageQueueWorker(
            IQueueMessageConsumer queueMessageConsumer,
            ILog log,
            IKonfigurasjon konfigurasjon) : base(log, konfigurasjon)
        {
            this.queueMessageConsumer = queueMessageConsumer;
            this.log = log;
        }

        protected override void Report(string message)
        {
            log.Info(message);
        }

        protected override void OnExecuting(CloudQueueMessage workItem)
        {
            //Do some work 
            var message = workItem.AsString;
            
            log.Debug("Nytt workitem: " + message);

            queueMessageConsumer.KonsumerMelding(workItem);

            //Used for testing the poison queue
            if (message == "fail")
                throw new Exception(message);

            Thread.Sleep(TimeSpan.FromSeconds(10));
        }
    }
}