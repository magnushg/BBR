using Castle.DynamicProxy;
using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Bouvet.BouvetBattleRoyale.Applikasjon.Owin.Interception
{
    public class RetryInterceptor : IInterceptor
    {
        public int MaxAttempts { get; set; }

        private bool isIntercepting = false;

        private void ProceedAndRetry(IInvocation invocation, int retryCount) 
        {
            try
            {
                invocation.Proceed();

                var task = invocation.ReturnValue as Task;
                if (task != null)
                {
                    Func<Task> taskProvider = () => GetTaskFromInvocation(invocation);

                    TaskHelper2.Retry(task, taskProvider, MaxAttempts, (Exception ex) => ShouldRetry(ex));
                }               
            }
            catch (Exception ex)
            {
                OnException(invocation, ex, retryCount);
            }
        }

        private Task GetTaskFromInvocation(IInvocation invocation)
        {
            invocation.Proceed();
            return invocation.ReturnValue as Task;
        }

        private bool ShouldRetry(Exception exception)
        {
            var strategy = new ConcurrencyProblemRetryStrategy(exception);
            return strategy.ShouldRetry;
        }

        private void RetryContinuation(Task task, Func<Task> taskProvider, int attemptsRemaining, Func<Exception, bool> shouldRetry)
        {
           
            if (task.IsFaulted)
            {
                if (attemptsRemaining > 0 && shouldRetry(task.Exception.InnerException))
                {
                    taskProvider()
                        .ContinueWith(retryTask => RetryContinuation(retryTask, taskProvider, --attemptsRemaining, shouldRetry))
                        ;
                }
            }
        }

        private void OnException(IInvocation invocation, Exception ex, int retryCount) 
        {

            if (ex is AggregateException && ex.InnerException != null)
                ex = ex.InnerException;

            if (ex.InnerException is DocumentClientException)
            {
                var documentException = (DocumentClientException)ex.InnerException;

                if (documentException.Error.Code == "PreconditionFailed")
                {
                    if (documentException.RetryAfter > TimeSpan.Zero)
                        Thread.Sleep(documentException.RetryAfter);

                    retryCount++;

                    if(retryCount > MaxAttempts)
                        throw ex;

                    ProceedAndRetry(invocation, retryCount);
                }
                else
                {
                    throw ex;
                }
            }
            else
            {
                throw ex;
            }
        }

        public void Intercept(IInvocation invocation)
        {
            if (!invocation.Method.GetCustomAttributes(typeof(HttpPostAttribute), false).Any())
            {
                invocation.Proceed();
                return;
            }

            if (isIntercepting)
                return;

            isIntercepting = true;

            ProceedAndRetry(invocation, 0);

            //int retryCount = 0;

            //while (retryCount <= ShouldTry)
            //{
            //    try
            //    {
            //        invocation.Proceed();
            //    }
            //    catch (Exception ex)
            //    {
            //        OnException(invocation, ex);

            //        if (ex.InnerException is DocumentClientException)
            //        {
            //            var documentException = (DocumentClientException)ex.InnerException;

            //            if (documentException.Error.Code == "PreconditionFailed")
            //            {
            //                if (documentException.RetryAfter > TimeSpan.Zero)
            //                    Thread.Sleep(documentException.RetryAfter);

            //                retryCount++;
            //                continue;
            //            }
            //        }

            //        // Passthrough: Raise original exception
            //        throw;
            //    }
            //}
        }
    }
}
