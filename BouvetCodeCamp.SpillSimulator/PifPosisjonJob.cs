using System;
using System.Net;
using System.Net.Http;
using System.Text;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.Domene.InputModels;
using Newtonsoft.Json;
using Quartz;

namespace BouvetCodeCamp.SpillSimulator
{
    public class PifPosisjonJob : IJob
    {
        public async void Execute(IJobExecutionContext context)
        {
            string ApiEndPointAddress = SpillKonfig.ApiBaseAddress + "/api/game/pif/sendpifposisjon/";
            var random = new Random();

            using (var httpClient = new HttpClient())
            {
                var modell = new PifPosisjonInputModell
                {
                    LagId = SpillKonfig.TestLagId,
                    Posisjon = SpillKonfig.Koordinater[random.Next(0,SpillKonfig.Koordinater.Count - 1)]
                };

                var modellSomJson = JsonConvert.SerializeObject(modell);

                var httpResponseMessage = await httpClient.PostAsync(
                    ApiEndPointAddress,
                    new StringContent(modellSomJson, Encoding.UTF8, "application/json"));

                Console.WriteLine(string.Format("Moved team to lat: {0}, lon: {1}", modell.Posisjon.Latitude, modell.Posisjon.Longitude));

            }
            
        }
    }
}