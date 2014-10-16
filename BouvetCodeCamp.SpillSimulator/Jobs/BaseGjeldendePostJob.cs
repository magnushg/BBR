using System;
using System.Net.Http;

using BouvetCodeCamp.Domene.OutputModels;
using Newtonsoft.Json;
using Quartz;

namespace BouvetCodeCamp.SpillSimulator.Jobs
{
    public class BaseGjeldendePostJob : Job, IJob
    {
        public async void Execute(IJobExecutionContext context)
        {
            string ApiEndPointAddress = SpillKonfig.ApiBaseAddress + "/api/game/base/hentgjeldendepost/" + SpillKonfig.LagId;

            PostOutputModell gjeldendePost;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);
                    var content = await httpResponseMessage.Content.ReadAsStringAsync();

                    SpillKonfig.GjeldendePost = JsonConvert.DeserializeObject<PostOutputModell>(content);

                    if (SpillKonfig.GjeldendePost == null)
                        Console.WriteLine("BASE Ingen flere poster å hente.");

                    Console.WriteLine("{0}: BASE Hentet ny gjeldende post med nummer {1}", SkrivTidsstempel(), SpillKonfig.GjeldendePost.Nummer);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("En feil skjedde under henting av gjeldende post: " + e.Message);
            }
        }
    }
}