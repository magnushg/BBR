using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BouvetCodeCamp.SpillSimulator.Schedulers;
using Quartz;
using Quartz.Impl;

namespace BouvetCodeCamp.SpillSimulator
{
    public class Program
    {
        static void Main(string[] args)
        {
            Common.Logging.LogManager.Adapter = new Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter { Level = Common.Logging.LogLevel.Info };
            try
            {
                var scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();
                var spillTilstandOppretter = new SpillTilstandOppretter();
                var pifScheduler = new PifScheduler(scheduler);
                var baseScheduler = new BaseScheduler(scheduler);

                pifScheduler.SchedulePifMoveJobs();
                pifScheduler.SchedulePifFinnPostJob();
                baseScheduler.ScheduleHentGjeldendePost();
                

                // Sov for å la oppgavene utføres
                Thread.Sleep(TimeSpan.FromSeconds(120));

                scheduler.Shutdown();
            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }
        }
    }
}
