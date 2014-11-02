using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bouvet.BouvetBattleRoyale.Applikasjon.Owin.Interception
{
    public static class TaskHelper
    {
        public static Task Retry(Task initialTask, Func<Task> taskProvider, int maxAttemps, Func<Exception, bool> shouldRetry)
        {
            return initialTask
                .ContinueWith(task => RetryContinuation(task, taskProvider, maxAttemps, shouldRetry), TaskContinuationOptions.ExecuteSynchronously);
        }    

        public static Task RetryContinuation(Task task, Func<Task> taskProvider, int attemptsRemaining, Func<Exception, bool> shouldRetry)
        {
            if (task.IsFaulted)
            {
                if (attemptsRemaining > 0 && shouldRetry(task.Exception.InnerException))
                {
                    return taskProvider()
                        .ContinueWith(retryTask => RetryContinuation(retryTask, taskProvider, --attemptsRemaining, shouldRetry), TaskContinuationOptions.ExecuteSynchronously);
                }
            }           
            return task;
        }
    }
}
