using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bouvet.BouvetBattleRoyale.Applikasjon.Owin.Interception
{
    public static class TaskHelper
    {
        public static Task Retry(Task initialTask, Func<Task> taskProvider, int maxAttemps, Func<Exception, IRetryStrategy> getRetryStrategy)
        {
            return initialTask
                .ContinueWith(task => RetryContinuation(task, taskProvider, maxAttemps, getRetryStrategy), TaskContinuationOptions.ExecuteSynchronously);
        }    

        public static Task RetryContinuation(Task task, Func<Task> taskProvider, int attemptsRemaining, Func<Exception, IRetryStrategy> getRetryStrategy)
        {
            if (task.IsFaulted)
            {
                var retryStrategy = getRetryStrategy(task.Exception.InnerException);

                if (attemptsRemaining > 0 && retryStrategy.ShouldRetry)
                {
                    if (retryStrategy.RetryAfter > TimeSpan.Zero)
                        Thread.Sleep(retryStrategy.RetryAfter);

                    return taskProvider()
                        .ContinueWith(retryTask => RetryContinuation(retryTask, taskProvider, --attemptsRemaining, getRetryStrategy), TaskContinuationOptions.ExecuteSynchronously);
                }
            }           
            return task;
        }       
    }
}
