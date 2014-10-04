using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Quartz;

namespace BouvetCodeCamp.SpillSimulator
{
    public class Program
    {
        static void Main(string[] args)
        {
            Common.Logging.LogManager.Adapter = new Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter { Level = Common.Logging.LogLevel.Info };
            try
            {
                using (var pifScheduler = new PifScheduler())
                {
                    pifScheduler.SchedulePifMoveJobs();
                }

            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }
        }
    }
}
