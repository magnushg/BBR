using System;

using Microsoft.Owin.Hosting;

namespace BouvetCodeCamp
{
    class Program
    {
        static void Main(string[] args)
        {
            const string BaseAddress = "http://localhost:2014";

            using (WebApp.Start<Startup>(BaseAddress))
            {
                Console.WriteLine("Server running at {0}", BaseAddress);
                Console.WriteLine("\r\nPress any key to stop server...");
                Console.ReadLine();
            }
        }
    }
}
