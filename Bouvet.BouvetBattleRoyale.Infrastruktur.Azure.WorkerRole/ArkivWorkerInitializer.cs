namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Azure.WorkerRole
{
    using Autofac;

    using Bouvet.BouvetBattleRoyale.Infrastruktur.Interfaces;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Logging;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Worker.Queues;

    public static class ArkivWorkerInitializer
    {
        public static void InitializeWorker()
        {
            var container = AutofacContainerBuilder.BuildAutofacContainer();

            InitializeLogging();

            var log = Log4NetLogger.HentLogger(typeof(WorkerRole));

            var queueMessageConsumer = container.Resolve<IQueueMessageConsumer>();

            var messageQueueWorker = new MessageQueueWorker(queueMessageConsumer, log);
            
            messageQueueWorker.Start();

            log.Info("Startet worker");
        }

        private static void InitializeLogging()
        {
            Log4NetLogger.InitialiserLogging<WorkerRole>();
        }
    }
}