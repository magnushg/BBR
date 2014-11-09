namespace Bouvet.BouvetBattleRoyale.SpillOppretter
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    using Bouvet.BouvetBattleRoyale.Domene;
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.CrossCutting;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Data;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Data.Repositories;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Logging;

    using Newtonsoft.Json;

    public class LagOppretter
    {
        private readonly int _antallLag;
        private readonly string _lagPosterPath;
        private readonly string _posterPath;
        private readonly LagRepository _lagRepository;
        private IEnumerable<Lag> _lagListe;
        private IEnumerable<Post> _poster;
        private IEnumerable<Lag> _lagListeMedPoster;

        private readonly List<string> hashes; 

        public LagOppretter(int antallLag, string lagPosterPath, string posterPath)
        {
            var log = Log4NetLogger.HentLogger(typeof(LagOppretter));

            _antallLag = antallLag;
            _lagPosterPath = lagPosterPath;
            _posterPath = posterPath;
            _lagRepository = new LagRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()), log);
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
                LagId = Sha256("Lag " + index + 1),
                LagNavn = "BouvetBBR L" + index,
                LagNummer = index,
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

        public string CreateGuid()
        {
            Guid guid = Guid.Empty;
            while (Guid.Empty == guid)
            {
                guid = Guid.NewGuid();
            }

            // Uses base64 encoding the guid.(Or  ASCII85 encoded)
            // But not recommend using Hex, as it is less efficient.
            return Convert.ToBase64String(guid.ToByteArray());
        }

        public string Sha256(string word)
        {
            SHA256Managed crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(word), 0, Encoding.ASCII.GetByteCount(word));
            foreach (byte bit in crypto)
            {
                hash += bit.ToString("x2");
            }
            return hash.Substring(0,7);
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
