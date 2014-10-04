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


            using (var httpClient = new HttpClient())
            {
                var modell = new PifPosisjonInputModell
                {
                    LagId = SpillKonfig.TestLagId,
                    Posisjon = new Koordinat
                    {
                        Latitude = "14.02",
                        Longitude = "11"
                    }
                };

                var modellSomJson = JsonConvert.SerializeObject(modell);

                var httpResponseMessage = await httpClient.PostAsync(
                    ApiEndPointAddress,
                    new StringContent(modellSomJson, Encoding.UTF8, "application/json"));

            }
            SpillKonfig.Times++;
            Console.WriteLine(string.Format("Moved team {0}", SpillKonfig.Times) );
        }
    }
}