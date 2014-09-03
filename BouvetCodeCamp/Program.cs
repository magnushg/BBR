using System;
using System.Net.Http;
using Microsoft.Owin.Hosting;

namespace BouvetCodeCamp
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:2014/";

            using (WebApp.Start<Startup>(baseAddress))
            {
                HttpClient client = new HttpClient();

                //var response = client.GetAsync(baseAddress + "api/timeseries/Adamselv").Result;

                //Console.WriteLine(response);
                //Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                Console.WriteLine("Server running at {0}", baseAddress);
                Console.WriteLine("\r\nPress any key to stop server...");
                Console.ReadLine();
            }
        }
    }
}
