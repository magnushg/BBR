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
        public int ShouldTry { get; set; }

        private void ProceedAndRetry(IInvocation invocation, int retryCount) 
        {
            try
            {
                invocation.Proceed();

                var task = invocation.ReturnValue as Task;
                if (task != null)
                {
                    if (retryCount < ShouldTry)
                    {
                        invocation.ReturnValue = task.ContinueWith(t =>
                        {                            
                            if (t.IsFaulted)
                                OnException(invocation, t.Exception, retryCount);
                        });
                    }
                }               
            }
            catch (Exception ex)
            {
                OnException(invocation, ex, retryCount);
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

                    if(retryCount > ShouldTry)
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
