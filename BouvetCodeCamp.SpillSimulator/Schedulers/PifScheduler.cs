using System.Collections.Generic;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.SpillSimulator.Jobs;
using Quartz;

namespace BouvetCodeCamp.SpillSimulator.Schedulers
{
    public class PifScheduler
    {
        private readonly IScheduler _scheduler;

        public PifScheduler(IScheduler scheduler)
        {
            _scheduler = scheduler;
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
        }

        public void SchedulePifFinnPostJob()
        {
            IJobDetail job = JobBuilder.Create<PifFinnPostJob>()
               .WithIdentity("finnPost", "pifGruppe")
               .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("finnTrigger", "pifGruppe")
                .StartAt(DateBuilder.FutureDate(20, IntervalUnit.Second))
                 .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(30)
                    .WithRepeatCount(10))
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }
    }
}