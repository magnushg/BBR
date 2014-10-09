using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
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
            //Common.Logging.LogManager.Adapter = new Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter { Level = Common.Logging.LogLevel.Info };
            //Æ \x92 \u00C6 Ø \x9D \u00D8 Å \x8F \u00C5";
            var spillTilstandOppretter = new SpillTilstandOppretter();
            try
            {
                Console.WriteLine("\r\nBouvet Battle Royale Simulator");
                Console.WriteLine("-------------------------------\r\n");
                Console.WriteLine("Tilgjengelige lag ID-er: {0}\r\n", string.Join(",", SpillKonfig.LagMedPostkoder.Keys.Select(x => x)));
                Console.Write("Lag ID for laget du vil kjøre simulatoren for: ");
                var lagId = Console.ReadLine();

                if (lagId == null || !SpillKonfig.LagMedPostkoder.ContainsKey(lagId))
                {
                    Console.WriteLine("Lag ID ikke funnet, avlutter...");
                    Environment.Exit(0);
                }

                SpillKonfig.LagId = lagId;

                Console.WriteLine("Kjører simulator for lag ID {0}\r\n", lagId);

                var scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();
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
