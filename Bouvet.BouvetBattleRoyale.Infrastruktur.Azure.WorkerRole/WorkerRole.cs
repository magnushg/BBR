using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.ServiceRuntime;

namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Azure.WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("Bouvet.BouvetBattleRoyale.Infrastruktur.Azure.WorkerRole is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();
                
            ArkivWorkerInitializer.InitializeWorker();

            Trace.TraceInformation("Bouvet.BouvetBattleRoyale.Infrastruktur.Azure.WorkerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("Bouvet.BouvetBattleRoyale.Infrastruktur.Azure.WorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("Bouvet.BouvetBattleRoyale.Infrastruktur.Azure.WorkerRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                
                await Task.Delay(1000);
            }
        }
    }
}