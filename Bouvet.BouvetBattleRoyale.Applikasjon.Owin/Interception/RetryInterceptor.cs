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

        public void Intercept(IInvocation invocation)
        {
            if (!invocation.Method.GetCustomAttributes(typeof(HttpPostAttribute), false).Any())
            {
                invocation.Proceed();
                return;
            }

            ProceedAndRetry(invocation, 0);
        }

        private void ProceedAndRetry(IInvocation invocation, int retryCount) 
        {
            try
            {
                invocation.Proceed();

                var task = invocation.ReturnValue as Task;
                if (task != null)
                {
                    // Step into async territory...
                    Func<Task> taskProvider = () => GetTaskFromInvocation(invocation);
                    Func<Exception, IRetryStrategy> shouldRetry = (Exception ex) => new ConcurrencyProblemRetryStrategy(ex);

                    TaskHelper.Retry(task, taskProvider, MaxAttempts, shouldRetry);
                }               
            }
            catch (Exception ex)
            {
                var strategy = new ConcurrencyProblemRetryStrategy(ex);

                if (!strategy.ShouldRetry)
                    throw;

                if (strategy.RetryAfter > TimeSpan.Zero)
                    Thread.Sleep(strategy.RetryAfter);

                retryCount++;

                if (retryCount > MaxAttempts)
                    throw ex;

                ProceedAndRetry(invocation, retryCount);
            }
        }

        private Task GetTaskFromInvocation(IInvocation invocation)
        {
            invocation.Proceed();
            return invocation.ReturnValue as Task;
        }             
    }
}
