namespace Bouvet.BouvetBattleRoyale.SpillOppretter
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Threading;

    using BouvetCodeCamp.SpillOppretter;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bouvet Battle Royale Game Creator");
            Console.WriteLine("----------------------------------\r\n");
            Console.Write("Create new game for {0} in {1} (y/n): ", ConfigurationManager.AppSettings["location"], ConfigurationManager.AppSettings["DocumentDbDatabaseNavn"]);
            var answer = Console.ReadLine();

            if (answer == "y" || answer == "yes")
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

                Console.WriteLine("Oppretter lag...");
                lagoppretter.OpprettLag(poster);

                Thread.Sleep(TimeSpan.FromSeconds(5));

                var antallLagOpprettet = lagoppretter.HentAlleLag();
                var ønsketAntallLag = ConfigurationManager.AppSettings["numberOfTeams"];

                Console.WriteLine("{0} av {1} lag opprettet", antallLagOpprettet.Count(), ønsketAntallLag);

                Console.WriteLine("\r\nPress any key to exit...");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Exiting game creator...");
                Environment.Exit(0);
            }
        }
            
    }
}