namespace Bouvet.BouvetBattleRoyale.Integrasjonstester
{
    using System;
    using System.Net.Http.Headers;

    public class TestManager
    {
        public static AuthenticationHeaderValue OpprettBasicHeader(string brukernavn, string passord)
        {
            byte[] byteArrayMedAuthorizationToken = System.Text.Encoding.UTF8.GetBytes(brukernavn + ":" + passord);

            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArrayMedAuthorizationToken));
        }
    }
}