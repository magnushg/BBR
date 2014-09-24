using System;
using System.Collections.Generic;
using System.Linq;

using BouvetCodeCamp.Domene.Entiteter;

namespace BouvetCodeCamp.KartdataImport
{
    using BouvetCodeCamp.Infrastruktur;
    using BouvetCodeCamp.Infrastruktur.DataAksess;
    using BouvetCodeCamp.Infrastruktur.DataAksess.Repositories;

    class Program
    {
        static void Main(string[] args)
        {
            var mapdataConverter = new KartdataKonverterer();

            Console.WriteLine("Initializing Document Db");
            
            Console.WriteLine("Converting data and saving to database");
            
            var mapdata = mapdataConverter.KonverterKartdata().ToList();

            var kartdataLagring = new KartdataLagring();

            kartdataLagring.SlettAlleKartdata();
            kartdataLagring.LagreKartdata(mapdata);

            Console.WriteLine("Done processing {0} map data points", mapdata.Count);
            Console.WriteLine("\r\nPress any key to exit...");
            Console.ReadLine();
        }
    }

    internal class KartdataLagring
    {
        private readonly PostRepository _postRepository;
        public KartdataLagring()
        {
            _postRepository = new PostRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));
        }

        public void LagreKartdata(IEnumerable<Post> poster)
        {
            poster.ToList().ForEach(async x =>
            {
                await _postRepository.Opprett(x);
            });
        }

        public async void SlettAlleKartdata()
        {
            var allePoster = await _postRepository.HentAlle();

            foreach (var post in allePoster)
            {
                await _postRepository.Slett(post);
            }
        }
    }
}