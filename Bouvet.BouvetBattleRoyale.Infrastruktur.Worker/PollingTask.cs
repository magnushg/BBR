namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Worker
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Bouvet.BouvetBattleRoyale.Infrastruktur.Interfaces;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Worker.Brisebois.WindowsAzure;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Worker.Properties;

    using log4net;

    /// <summary>
    /// Details: http://alexandrebrisebois.wordpress.com/2013/02/19/polling-tasks-are-great-building-blocks-for-windows-azure-roles/
    /// </summary>
    public abstract class PollingTask<TWorkItem> : IWorkerProcess, IDisposable
    {
        private readonly ILog log;

        private Task internalTask;
        private readonly CancellationTokenSource source;
        private int attempts;

        private const int MaxDelayInSeconds = 1024;

        protected PollingTask(ILog log)
        {
            this.log = log;
            source = new CancellationTokenSource();
        }

        protected abstract void Report(string message);

        public void Start()
        {
            if (internalTask != null)
                throw new PollingTaskException("Task is already running");

            internalTask = Task.Run(() =>
            {
                while (!source.IsCancellationRequested)
                {
                    TryExecuteWorkItems();

                    Report(Resources.Heart_Beat);
                }
            }, source.Token);
        }

        private void TryExecuteWorkItems()
        {
            try
            {
                var files = TryGetWork();

                if (files.Any())
                {
                    ResetAttempts();
                    files.AsParallel()
                         .ForAll(ExecuteWork);
                }
                else
                    BackOff();
            }
            catch (AggregateException ex)
            {
                log.Warn("Exception ved meldingshenting: " + ex.Message, ex);

                Report(ex.ToString());
                
                if (Debugger.IsAttached)
                    Trace.TraceError(ex.ToString());
            }
        }

        private void ExecuteWork(TWorkItem workItem)
        {
            Report(Resources.Started_work_on_work_item);
            var w = new Stopwatch();
            w.Start();
            Execute(workItem);
            w.Stop();
            Report(string.Format(CultureInfo.InvariantCulture,
                                 Resources.Completed_work_on_work_item,
                                 w.Elapsed.TotalMinutes));
            Completed(workItem);
        }

        protected void BackOff()
        {
            attempts++;

            var seconds = GetTimeoutAsTimeSpan();

            if (seconds.TotalSeconds > 0)
                Report(string.Format("{0}{1}s", Resources.Sleep_for, seconds.TotalSeconds));

            Thread.Sleep(seconds);
        }

        private TimeSpan GetTimeoutAsTimeSpan()
        {
            var timeout = DelayCalculator.ExponentialDelay(attempts, MaxDelayInSeconds);

            var seconds = TimeSpan.FromSeconds(timeout);
            return seconds;
        }

        protected abstract void Execute(TWorkItem workItem);
        protected abstract void Completed(TWorkItem workItem);
        protected abstract ICollection<TWorkItem> TryGetWork();

        public void Cancel()
        {
            source.Cancel();
            internalTask = null;
        }

        public void ResetAttempts()
        {
            attempts = 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                internalTask.Dispose();
                source.Dispose();
            }
            internalTask = null;
        }
    }
}