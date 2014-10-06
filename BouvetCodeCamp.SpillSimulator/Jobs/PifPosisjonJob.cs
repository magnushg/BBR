using System;
using System.Net.Http;
using System.Text;
using BouvetCodeCamp.Domene.InputModels;
using Newtonsoft.Json;
using Quartz;

namespace BouvetCodeCamp.SpillSimulator.Jobs
{
    public class PifPosisjonJob : IJob
    {
        public async void Execute(IJobExecutionContext context)
        {
            string ApiEndPointAddress = SpillKonfig.ApiBaseAddress + "/api/game/pif/sendpifposisjon/";
            if (SpillKonfig.KoordinatIndex > SpillKonfig.Koordinater.Count - 1)
            {
                SpillKonfig.KoordinatIndex = 0;
            }
            
            using (var httpClient = new HttpClient())
            {
                var modell = new PifPosisjonInputModell
                {
                    LagId = SpillKonfig.TestLagId,
                    Posisjon = SpillKonfig.Koordinater[SpillKonfig.KoordinatIndex]
                };

                var modellSomJson = JsonConvert.SerializeObject(modell);

                var httpResponseMessage = await httpClient.PostAsync(
                    ApiEndPointAddress,
                    new StringContent(modellSomJson, Encoding.UTF8, "application/json"));

                Console.WriteLine(string.Format("PIF flyttet til posisjon lat: {0}, lon: {1}", modell.Posisjon.Latitude, modell.Posisjon.Longitude));
                SpillKonfig.KoordinatIndex++;
            }
            
        }
    }
}