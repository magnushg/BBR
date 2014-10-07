using System;
using System.Net.Http;
using Microsoft.Owin.Hosting;

namespace BouvetCodeCamp
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://+:80";

            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine("Server running at {0}", baseAddress);
                Console.WriteLine("\r\nPress any key to stop server...");
                Console.ReadLine();
            }
        }
    }
}
