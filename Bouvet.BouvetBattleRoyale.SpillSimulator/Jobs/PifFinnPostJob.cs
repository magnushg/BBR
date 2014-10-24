using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BouvetCodeCamp.Domene.Entiteter;

using Newtonsoft.Json;
using Quartz;

namespace BouvetCodeCamp.SpillSimulator.Jobs
{
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Domene.InputModels;

    public class PifFinnPostJob : Job, IJob
    {
        public async void Execute(IJobExecutionContext context)
        {

            using (var httpClient = new HttpClient())
            {
                if (SpillKonfig.GjeldendePost != null)
                {
                    string ApiEndPointAddress = SpillKonfig.ApiBaseAddress + "/api/game/pif/sendpostkode/";
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

                    var output = string.Format(httpResponseMessage.StatusCode == HttpStatusCode.OK
                            ? "PIF validerte post {0}"
                            : "PIF Validering av post {0} mislyktes", modell.Postnummer);

                    Console.WriteLine("{0}: {1}", SkrivTidsstempel(), output);
                }
                else
                {
                    Console.WriteLine("{0}: PIF har funnet alle poster", SkrivTidsstempel());
                }
            }
        }
    }
}
