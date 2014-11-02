using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bouvet.BouvetBattleRoyale.Applikasjon.Owin.Interception
{
    public interface IRetryStrategy
    {
        bool ShouldRetry { get; }
        TimeSpan RetryAfter { get; }
    }

    public class ConcurrencyProblemRetryStrategy : IRetryStrategy
    {
        public bool ShouldRetry { get; set; }
        public TimeSpan RetryAfter { get; set;}

        public ConcurrencyProblemRetryStrategy(Exception ex)
        {
            ShouldRetry = false;
            RetryAfter = TimeSpan.Zero;

            if (ex is AggregateException && ex.InnerException != null)
                ex = ex.InnerException;

            if (ex.InnerException is DocumentClientException)
            {
                var documentException = (DocumentClientException)ex.InnerException;

                if (documentException.Error.Code == "PreconditionFailed")
                {
                    RetryAfter = documentException.RetryAfter;
                    ShouldRetry = true;
                }
            }
        }        
    }
}
