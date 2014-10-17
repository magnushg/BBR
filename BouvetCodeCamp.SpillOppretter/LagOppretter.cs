using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BouvetCodeCamp.Domene;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.Infrastruktur.DataAksess;
using BouvetCodeCamp.Infrastruktur.DataAksess.Repositories;
using Newtonsoft.Json;

namespace BouvetCodeCamp.SpillOppretter
{
    using System.Threading;

    public class LagOppretter
    {
        private readonly int _antallLag;
        private readonly string _lagPosterPath;
        private readonly string _posterPath;
        private readonly LagRepository _lagRepository;
        private IEnumerable<Lag> _lagListe;
        private IEnumerable<Post> _poster;
        private IEnumerable<Lag> _lagListeMedPoster;

        private List<string> hashes; 

        public LagOppretter(int antallLag, string lagPosterPath, string posterPath)
        {
            _antallLag = antallLag;
            _lagPosterPath = lagPosterPath;
            _posterPath = posterPath;
            _lagRepository = new LagRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));
            _lagListe = new List<Lag>();

            hashes = new List<string>();
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
                LagId = ShaChecksum("Lag " + index + 1, index),
                LagNavn = "BouvetBBR L" + index,
                LagNummer = index,
                LoggHendelser = new List<LoggHendelse>(),
                Meldinger = new List<Melding>(),
                Poster = new List<LagPost>(),
                PifPosisjoner = new List<PifPosisjon>(),
                Poeng = 0
            }).ToList();
        }

        public string ShaChecksum(string input, int index)
        {
            var sha = SHA256.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = sha.ComputeHash(inputBytes);

            var generertHash = GenererHash(index, hash);

            // TODO: Forbedres, .Contains virker ikke. sørg for unike verdier i liste
            // Regenerer til alle hashes er unike
            if (hashes.Contains(generertHash))
            {
                index += 42;
                
                generertHash = GenererHash(index, hash);
            }
            
            hashes.Add(generertHash);
            
            return hashes.Last();
        }

        private static string GenererHash(int index, byte[] hash)
        {
            return index <= hash.Length - 1?
                       hash[index].ToString(CultureInfo.InvariantCulture) 
                       : hash.Last().ToString(CultureInfo.InvariantCulture);
        }

        private IEnumerable<Lag> TilordnePosterTilLagListe()
        {
            var lagPosterJson = File.ReadAllText(_lagPosterPath, Encoding.UTF8);
            var lagPoster = JsonConvert.DeserializeObject<IEnumerable<LagPoster>>(lagPosterJson);

            var posterMedKoderJson = File.ReadAllText(_posterPath, Encoding.UTF8);
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
            var alleLag = _lagRepository.HentAlle();

            foreach (var lag in alleLag)
            {
                await _lagRepository.Slett(lag);
            }

            lagListe.ToList().ForEach(async x =>
            {
                await _lagRepository.Opprett(x);
            });
        }

        public IEnumerable<Lag> HentAlleLag()
        {
            return _lagRepository.HentAlle();
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
