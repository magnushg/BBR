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
        public void Intercept(IInvocation invocation)
        {
            if (!invocation.Method.GetCustomAttributes(typeof(HttpPostAttribute), false).Any())
            {
                invocation.Proceed();
                return;
            }

            int maxRetry = 3;
            int retryCount = 0;

            while (retryCount <= maxRetry)
            {
                try
                {
                    invocation.Proceed();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException is DocumentClientException)
                    {
                        var documentException = (DocumentClientException)ex.InnerException;

                        if (documentException.Error.Code == "PreconditionFailed")
                        {
                            if (documentException.RetryAfter > TimeSpan.Zero)
                                Thread.Sleep(documentException.RetryAfter);

                            retryCount++;
                            continue;
                        }
                    }

                    // Passthrough: Raise original exception
                    throw;
                }
            }
        }
    }
}
