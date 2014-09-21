using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BouvetCodeCamp.Dataaksess;
using BouvetCodeCamp.Dataaksess.Repositories;
using BouvetCodeCamp.Felles.Entiteter;
using BouvetCodeCamp.Felles.Konfigurasjon;

namespace BouvetCodeCamp.KartdataImport
{
    class Program
    {
        static void Main(string[] args)
        {
            var mapdataConverter = new KartdataKonverterer();
            Console.WriteLine("Initializing Document Db");
            
            Console.WriteLine("Converting data and saving to database");
            var kartdataLagring = new KartdataLagring();
            var mapdata = mapdataConverter.KonverterKartdata().ToList();
            kartdataLagring.LagreKartdata(mapdata);
            Console.WriteLine("Done processing {0} map data points", mapdata.Count);
            Console.WriteLine("\r\nPress any key to exit...");
            Console.ReadLine();
        }

        
    }

    internal class KartdataLagring
    {
        private PostRepository _postRepository;
        public KartdataLagring()
        {
            _postRepository = new PostRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));
        }

        public async void LagreKartdata(IEnumerable<Post> poster)
        {
            poster.ToList().ForEach(async x =>
            {
                await _postRepository.Opprett(x);
            });
        }
    }
}
