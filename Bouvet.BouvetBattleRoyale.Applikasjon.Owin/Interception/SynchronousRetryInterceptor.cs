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
    public class SynchronousRetryInterceptor : IInterceptor
    {
        public int MaxAttempts { get; set; }

        public void Intercept(IInvocation invocation)
        {
            if (!invocation.Method.GetCustomAttributes(typeof(HttpPostAttribute), false).Any())
            {
                invocation.Proceed();
                return;
            }

            int retryCount = 0;

            while (retryCount <= MaxAttempts)
            {
                try
                {
                    invocation.Proceed();
                }
                catch (Exception ex)
                {
                    var strategy = new ConcurrencyProblemRetryStrategy(ex);

                    if (!strategy.ShouldRetry)
                        throw;

                    if (strategy.RetryAfter > TimeSpan.Zero)
                        Thread.Sleep(strategy.RetryAfter);

                    retryCount++;
                }
            }
        }
    }
}
