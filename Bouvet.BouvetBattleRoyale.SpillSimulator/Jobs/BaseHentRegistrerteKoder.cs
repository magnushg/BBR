using System;
using System.Net.Http;

using Newtonsoft.Json;
using Quartz;

namespace BouvetCodeCamp.SpillSimulator.Jobs
{
    public class BaseHentRegistrerteKoder : Job, IJob
    {
        public async void Execute(IJobExecutionContext context)
        {
            string ApiEndPointAddress = SpillKonfig.ApiBaseAddress + "/api/game/base/hentregistrertekoder/" + SpillKonfig.LagId;

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);
                    var content = await httpResponseMessage.Content.ReadAsStringAsync();

                    var koder = JsonConvert.DeserializeObject<string[]>(content);

                    if (string.IsNullOrEmpty(content))
                        Console.WriteLine("BASE Ingen koder registrert");

                    Console.WriteLine("{0}: BASE Koder som er oppdaget {1}", SkrivTidsstempel(), string.Join(", ", koder));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: {0}", e.StackTrace);
                }
                
            }
        }
    }
}
