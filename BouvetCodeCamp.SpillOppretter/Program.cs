using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace BouvetCodeCamp.SpillOppretter
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = string.Format("importData/{0}", ConfigurationManager.AppSettings["location"]);

            var mapdataConverter = new JSONKartdataConverter(filePath + "/poster.json");

            Console.WriteLine("Initializing Document Db");
            
            var kartdataLagring = new KartdataLagring();
            var lagoppretter = new LagOppretter(Convert.ToInt32(ConfigurationManager.AppSettings["numberOfTeams"]), filePath + "/lagPoster.json", filePath + "/koder.json");

            Console.WriteLine("Converting data and saving to database");

            var mapdata = mapdataConverter.KonverterKartdata().ToList();

            kartdataLagring.SlettAlleKartdata();
            var poster = kartdataLagring.LagreKartdata(mapdata);

            Console.WriteLine("Done processing {0} map data points", mapdata.Count);

            Console.WriteLine("Oppretter lag");
            lagoppretter.OpprettLag(poster);

            Console.WriteLine("\r\nPress any key to exit...");
            Console.ReadLine();
        }
    }
}