using System;
using System.Collections.Generic;
using System.Threading;
using BouvetCodeCamp.Domene.Entiteter;
using Quartz;
using Quartz.Impl;

namespace BouvetCodeCamp.SpillSimulator
{
    public class PifScheduler
    {
        private readonly IScheduler _scheduler;

        public PifScheduler(IScheduler scheduler)
        {
            _scheduler = scheduler;
            OpprettKoordinater();
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

        private static void OpprettKoordinater()
        {
            SpillKonfig.Koordinater = new List<Koordinat>
            {
                new Koordinat
                {
                    Latitude = "59.67878",
                    Longitude = "10.60392"
                },
                new Koordinat
                {
                    Latitude = "59.67944",
                    Longitude = "10.6042"
                },
                new Koordinat
                {
                    Latitude = "59.68023411",
                    Longitude = "10.6041259971"
                },
                new Koordinat
                {
                    Latitude = "59.6804558114",
                    Longitude = "10.60457",
                }
            };
        }
    }
}