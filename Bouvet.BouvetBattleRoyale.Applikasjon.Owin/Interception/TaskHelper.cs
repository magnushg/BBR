using log4net;
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
        public static TaskContinuationOptions TaskContinuationOptions = TaskContinuationOptions.ExecuteSynchronously;

        public static Task Retry(Task initialTask, Func<Task> taskProvider, int maxAttemps, Func<Exception, IRetryStrategy> getRetryStrategy, ILog logger)
        {
            return initialTask
                .ContinueWith(task => RetryContinuation(task, taskProvider, maxAttemps, getRetryStrategy, logger), TaskContinuationOptions);
        }

        public static Task RetryContinuation(Task task, Func<Task> taskProvider, int attemptsRemaining, Func<Exception, IRetryStrategy> getRetryStrategy, ILog logger)
        {
            if (task.IsFaulted)
            {
                var retryStrategy = getRetryStrategy(task.Exception.InnerException);

                if (attemptsRemaining > 0 && retryStrategy.ShouldRetry)
                {
                    if (retryStrategy.RetryAfter > TimeSpan.Zero)
                        Thread.Sleep(retryStrategy.RetryAfter); // Burde nok brukt noe Task.Delay-aktig her

                    logger.WarnFormat("{0}: Retrying faulted task. Attempts remaining: {1}", typeof(TaskHelper).FullName, attemptsRemaining);

                    return taskProvider()
                        .ContinueWith(retryTask => RetryContinuation(retryTask, taskProvider, --attemptsRemaining, getRetryStrategy, logger), TaskContinuationOptions);
                }

                if(attemptsRemaining == 0)
                    logger.WarnFormat("{0}: Retried faulted task. Giving up after last retry.", typeof(TaskHelper).FullName);
            }           
            return task;
        }       
    }
}
