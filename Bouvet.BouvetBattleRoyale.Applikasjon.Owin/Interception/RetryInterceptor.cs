using Castle.DynamicProxy;
using log4net;
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
        public static int DefaultMaxAttempts = 3;

        public int MaxAttempts { get; set; }

        private ILog _logger;

        public RetryInterceptor(ILog logger)
        {
            _logger = logger;

            MaxAttempts = DefaultMaxAttempts;
        }

        public void Intercept(IInvocation invocation)
        {
            if (!MethodShouldBeRetried(invocation))
            {
                invocation.Proceed();
                return;
            }

            ProceedAndRetry(invocation);
        }

        private static bool MethodShouldBeRetried(IInvocation invocation)
        {
            return invocation.Method.GetCustomAttributes(typeof(HttpPostAttribute), false).Any();
        }

        private void ProceedAndRetry(IInvocation invocation, int retryCount = 0) 
        {
            try
            {
                invocation.Proceed();

                var task = invocation.ReturnValue as Task;
                if (task != null)
                {
                    ProceedAndRetryAsync(invocation, task);
                }               
            }
            catch (Exception ex)
            {
                var strategy = new ConcurrencyProblemRetryStrategy(ex);

                if (!strategy.ShouldRetry)
                    throw;                

                retryCount++;

                if (retryCount > MaxAttempts)
                    throw ex;

                if (strategy.RetryAfter > TimeSpan.Zero)
                    Thread.Sleep(strategy.RetryAfter);

                ProceedAndRetry(invocation, retryCount);
            }
        }

        private void ProceedAndRetryAsync(IInvocation invocation, Task task)
        {            
            Func<Task> taskProvider = () =>
            {
                invocation.Proceed();
                return invocation.ReturnValue as Task;
            };

            Func<Exception, IRetryStrategy> shouldRetry = (Exception ex) => new ConcurrencyProblemRetryStrategy(ex);

            TaskHelper.Retry(task, taskProvider, MaxAttempts, shouldRetry, _logger);
        }            
    }
}
