using System;
using System.Threading;
using Quartz;
using Quartz.Impl;

namespace BouvetCodeCamp.SpillSimulator
{
    public class PifScheduler : IDisposable
    {
        private IScheduler _scheduler;

        public PifScheduler()
        {
            _scheduler = StdSchedulerFactory.GetDefaultScheduler();
            _scheduler.Start();
        }

        public void SchedulePifMoveJobs()
        {
            IJobDetail job = JobBuilder.Create<PifPosisjonJob>()
                .WithIdentity("flyttPif", "pifGruppe")
                .Build();
            
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger", "pifGruppe")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(5)
                    .RepeatForever())
                .Build();

            _scheduler.ScheduleJob(job, trigger);
            // Sov for å la oppgavene utføres
            Thread.Sleep(TimeSpan.FromSeconds(60));
        }

        public void Dispose()
        {
            _scheduler.Shutdown();
        }
    }
}