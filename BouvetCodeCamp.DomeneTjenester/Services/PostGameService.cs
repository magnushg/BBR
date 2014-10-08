namespace BouvetCodeCamp.DomeneTjenester.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using BouvetCodeCamp.Domene;
    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;

    public class PostGameService : IPostGameService
    {
        private readonly IKoordinatVerifier _koordinatVerifier;

        public PostGameService(IKoordinatVerifier koordinatVerifier)
        {
            _koordinatVerifier = koordinatVerifier;
        }

        public IEnumerable<LagPost> HentOppdagedePoster(Lag lag)
        {
            return lag.Poster.Where(post => post.PostTilstand == PostTilstand.Oppdaget);
        }

        public IEnumerable<LagPost> HentAllePosterForLag(Lag lag)
        {
            return lag.Poster;
        }

        /// <summary>
        /// Finn et lag sin kode og sett tilstanden til oppdaget, søket går på følgende kriterier:
        ///   1 - bokstav stemmer
        ///   2 - koordinatene er innenfor rekkevidde
        ///   3 - tilstanden ikke allerede er satt til oppdaget
        /// </summary>
        /// <returns>true hvis alle kriterier er oppfylt</returns>
        public bool SettKodeTilstandTilOppdaget(Lag lag, int postnummer, string kode, Koordinat koordinat)
        {
            var kandidater = lag.Poster.Where(k => k.Kode.Equals(kode, StringComparison.CurrentCultureIgnoreCase)
                                                   && k.Nummer == postnummer
                                                   && _koordinatVerifier.KoordinaterErNærHverandre(k.Posisjon, koordinat)
                                                   && k.PostTilstand.Equals(PostTilstand.Ukjent)).ToList();

            switch (kandidater.Count())
            {
                case 0:
                    return false;
                case 1:
                    kandidater.First().PostTilstand = PostTilstand.Oppdaget;
                    return true;
                default:
                    throw new AmbiguousMatchException("Flere koder funnet basert på kriteriene gitt");
            }
        }
    }
}