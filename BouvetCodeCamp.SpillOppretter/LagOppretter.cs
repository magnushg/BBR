using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BouvetCodeCamp.Domene;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.Infrastruktur.DataAksess;
using BouvetCodeCamp.Infrastruktur.DataAksess.Repositories;
using Newtonsoft.Json;

namespace BouvetCodeCamp.SpillOppretter
{
    public class LagOppretter
    {
        private readonly int _antallLag;
        private readonly LagRepository _lagRepository;

        public LagOppretter(int antallLag)
        {
            _antallLag = antallLag;
            _lagRepository = new LagRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));
        }

        public void OpprettLag()
        {
            var lagJson = File.ReadAllText("importData/lagData.json", Encoding.UTF8);
            var lagPosterJson = File.ReadAllText("importData/lagPoster.json", Encoding.UTF8);

            var lagListe = Enumerable.Range(1, _antallLag).Select(index => new Lag
            {
                LagId = Guid.NewGuid().ToString(),
                LagNavn = "BouvetBBR L" + index,
                LagNummer = index,
                LoggHendelser = new List<LoggHendelse>(),
                Meldinger = new List<Melding>(),
                Poster = new List<LagPost>(),
                PifPosisjoner = new List<PifPosisjon>(),
                Poeng = 0
            });
            //var lagListe = JsonConvert.DeserializeObject<IEnumerable<Lag>>(lagJson);
            var lagPoster = JsonConvert.DeserializeObject<IEnumerable<LagPoster>>(lagPosterJson);
            
            LagreLagliste(lagListe);

        }

        public async void LagreLagliste(IEnumerable<Lag> lagListe)
        {
            await _lagRepository.SlettAlle();
            lagListe.ToList().ForEach(async x =>
            {
                await _lagRepository.Opprett(x);
            });
        }
    }

    public class LagPoster
    {
        public int Lagnummer { get; set; }
        public string[] Poster { get; set; }
    }

}
