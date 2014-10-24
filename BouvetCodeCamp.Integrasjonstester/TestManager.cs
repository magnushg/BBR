namespace BouvetCodeCamp.Integrasjonstester
{
    using System;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    public class TestManager
    {
        public static async Task<int> RetryUntilSuccessOrTimeout(Func<Task<int>> task, TimeSpan timeSpan, int ønsketAntall)
        {
            var result = 0;
            var secondsElapsed = 0;

            const int WaitStepInMilliseconds = 1000;

            while ((result < ønsketAntall) && (secondsElapsed < timeSpan.TotalMilliseconds))
            {
                Thread.Sleep(WaitStepInMilliseconds);
                secondsElapsed += WaitStepInMilliseconds;

                result = await task();
            }
            return result;
        }

        public static AuthenticationHeaderValue OpprettBasicHeader(string brukernavn, string passord)
        {
            byte[] byteArrayMedAuthorizationToken = System.Text.Encoding.UTF8.GetBytes(brukernavn + ":" + passord);

            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArrayMedAuthorizationToken));
        }
    }
}
