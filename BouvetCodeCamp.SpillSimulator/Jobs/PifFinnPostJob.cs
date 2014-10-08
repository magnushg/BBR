using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.Domene.InputModels;
using Newtonsoft.Json;
using Quartz;

namespace BouvetCodeCamp.SpillSimulator.Jobs
{
    public class PifFinnPostJob : IJob
    {
        public async void Execute(IJobExecutionContext context)
        {

            using (var httpClient = new HttpClient())
            {
                const string ApiEndPointAddress = SpillKonfig.ApiBaseAddress + "/api/game/pif/sendpostkode/";
                var gjeldendePostNummer = SpillKonfig.GjeldendePost.Nummer;
                var postKoder = SpillKonfig.LagMedPostkoder[SpillKonfig.LagId];
                var modell = new PostInputModell
                {
                    Kode = postKoder.ContainsKey(gjeldendePostNummer) ? postKoder[gjeldendePostNummer] : "nogame",
                    Postnummer = SpillKonfig.GjeldendePost.Nummer,
                    Koordinat =
                        new Koordinat(SpillKonfig.GjeldendePost.Posisjon.Longitude, SpillKonfig.GjeldendePost.Posisjon.Latitude),
                    LagId = SpillKonfig.LagId
                };

                var modellSomJson = JsonConvert.SerializeObject(modell);

                var httpResponseMessage = await httpClient.PostAsync(
                    ApiEndPointAddress,
                    new StringContent(modellSomJson, Encoding.UTF8, "application/json"));

                Console.WriteLine(
                    httpResponseMessage.StatusCode == HttpStatusCode.OK
                        ? "PIF validerte post {0}"
                        : "PIF Validering av post {0} mislyktes", modell.Postnummer);
            }
        }
    }
}
