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
        private IEnumerable<Lag> _lagListe;
        private IEnumerable<Post> _poster;
        private IEnumerable<Lag> _lagListeMedPoster;

        public LagOppretter(int antallLag)
        {
            _antallLag = antallLag;
            _lagRepository = new LagRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));
            _lagListe = new List<Lag>();
        }

        public IEnumerable<Lag> OpprettLag(IEnumerable<Post> poster)
        {
            _lagListe = OpprettLagListeMedTommeLag();
            _poster = poster;

            _lagListeMedPoster = TilordnePosterTilLagListe();
            var lagListeMedPosterOgSekvensNummer = TilordneSekvensNummerTilPostListe();

            LagreLagliste(lagListeMedPosterOgSekvensNummer);

            return _lagListe;

        }

        private IEnumerable<Lag> OpprettLagListeMedTommeLag()
        {
            return Enumerable.Range(1, _antallLag).Select(index => new Lag
            {
                LagId = Guid.NewGuid().ToString(),
                LagNavn = "BouvetBBR L" + index,
                LagNummer = index,
                LoggHendelser = new List<LoggHendelse>(),
                Meldinger = new List<Melding>(),
                Poster = new List<LagPost>(),
                PifPosisjoner = new List<PifPosisjon>(),
                Poeng = 0
            }).ToList();
        }

        private IEnumerable<Lag> TilordnePosterTilLagListe()
        {
            var lagPosterJson = File.ReadAllText("importData/lagPoster.json", Encoding.UTF8);
            var lagPoster = JsonConvert.DeserializeObject<IEnumerable<LagPoster>>(lagPosterJson);

            var posterMedKoderJson = File.ReadAllText("importData/koder.json", Encoding.UTF8);
            var posterMedKoder = JsonConvert.DeserializeObject<IEnumerable<PosterMedKoder>>(posterMedKoderJson);


           return  _lagListe.Select(lag => new Lag
            {
                LagId = lag.LagId,
                LagNavn = lag.LagNavn,
                LagNummer = lag.LagNummer,
                Poeng = lag.Poeng,
                PifPosisjoner = lag.PifPosisjoner,
                LoggHendelser = lag.LoggHendelser,
                Meldinger = lag.Meldinger,
                Poster = lagPoster
                     .FirstOrDefault(lagPost => lagPost.Lagnummer == lag.LagNummer)
                     .Poster.Select(post => OpprettLagPost(_poster.FirstOrDefault(p => p.Nummer == post), posterMedKoder, lag.LagNummer)).ToList()

            }).ToList();
        }

        private LagPost OpprettLagPost(Post post, IEnumerable<PosterMedKoder> posterMedKoder, int lagNummer)
        {
            return new LagPost
            {
                Altitude = post.Altitude,
                Beskrivelse = post.Beskrivelse,
                Bilde = post.Bilde,
                Kilde = post.Kilde,
                Navn = post.Navn,
                Nummer = post.Nummer,
                Posisjon = post.Posisjon,
                PostTilstand = PostTilstand.Ukjent,
                Kode = posterMedKoder.FirstOrDefault(p => p.Postnr == post.Nummer).Koder[lagNummer-1]
            };
        }

        private IEnumerable<Lag> TilordneSekvensNummerTilPostListe()
        {
            return _lagListeMedPoster.Select(lag => new Lag
            {
                LagId = lag.LagId,
                LagNavn = lag.LagNavn,
                LagNummer = lag.LagNummer,
                LoggHendelser = lag.LoggHendelser,
                Meldinger = lag.Meldinger,
                PifPosisjoner = lag.PifPosisjoner,
                Poeng = lag.Poeng,
                Poster = lag.Poster.Select((post, index) => new LagPost
                {
                    Altitude = post.Altitude,
                    Beskrivelse = post.Beskrivelse,
                    Bilde = post.Bilde,
                    Kilde = post.Kilde,
                    Kode = post.Kode,
                    Navn = post.Navn,
                    Nummer = post.Nummer,
                    Posisjon = post.Posisjon,
                    PostTilstand = post.PostTilstand,
                    Sekvensnummer = index

                }).ToList()
            });
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
        public int[] Poster { get; set; }
    }


    public class PosterMedKoder
    {
        public int Postnr { get; set; }
        public string[] Koder { get; set; }
    }


}
