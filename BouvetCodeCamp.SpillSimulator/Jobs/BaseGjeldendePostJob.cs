using System;
using System.Net.Http;
using System.Text;
using BouvetCodeCamp.Domene.InputModels;
using BouvetCodeCamp.Domene.OutputModels;
using Newtonsoft.Json;
using Quartz;

namespace BouvetCodeCamp.SpillSimulator.Jobs
{
    public class BaseGjeldendePostJob : IJob
    {
        public async void Execute(IJobExecutionContext context)
        {
            string ApiEndPointAddress = SpillKonfig.ApiBaseAddress + "/api/game/base/hentgjeldendepost/" + SpillKonfig.TestLagId;

            PostOutputModell gjeldendePost;

            using (var httpClient = new HttpClient())
            {
                var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                SpillKonfig.GjeldendePost = JsonConvert.DeserializeObject<PostOutputModell>(content);

                Console.WriteLine("BASE Hentet ny gjeldende post med nummer {0}", SpillKonfig.GjeldendePost.Nummer);
            }
        }
    }
}
