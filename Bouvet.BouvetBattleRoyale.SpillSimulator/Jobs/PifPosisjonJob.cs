﻿using System;
using System.Net.Http;
using System.Text;

using Newtonsoft.Json;
using Quartz;

namespace BouvetCodeCamp.SpillSimulator.Jobs
{
    using Bouvet.BouvetBattleRoyale.Domene.InputModels;

    public class PifPosisjonJob : Job, IJob
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
                    LagId = SpillKonfig.LagId,
                    Posisjon = SpillKonfig.Koordinater[SpillKonfig.KoordinatIndex]
                };

                var modellSomJson = JsonConvert.SerializeObject(modell);

                var httpResponseMessage = await httpClient.PostAsync(
                    ApiEndPointAddress,
                    new StringContent(modellSomJson, Encoding.UTF8, "application/json"));

                Console.WriteLine(string.Format("{0}: PIF flyttet til posisjon lat: {1}, lon: {2}", SkrivTidsstempel(), modell.Posisjon.Latitude, modell.Posisjon.Longitude));
                SpillKonfig.KoordinatIndex++;
            }
            
        }
    }
}