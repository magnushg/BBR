using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BouvetCodeCamp.Domene;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester.Interfaces;

namespace BouvetCodeCamp.DomeneTjenester
{
    public class PostGameService : IPostGameService
    {
        private readonly ILagGameService _lagGameService;
        private readonly IService<Lag> _lagService;
        private readonly IKoordinatVerifier _koordinatVerifier;

        public PostGameService(
            ILagGameService lagGameService,
            IService<Lag> lagService,
            IKoordinatVerifier koordinatVerifier)
        {
            _lagGameService = lagGameService;
            _lagService = lagService;
            _koordinatVerifier = koordinatVerifier;
        }

        public IEnumerable<LagPost> HentOppdagedePoster(string lagId)
        {
            return _lagGameService.HentLagMedLagId(lagId).Poster.Where(post => post.PostTilstand == PostTilstand.Oppdaget);
        }

        public IEnumerable<LagPost> HentAllePosterForLag(string lagId)
        {
            var lag = _lagGameService.HentLagMedLagId(lagId);

            return lag.Poster;
        }

        /// <summary>
        /// Finn et lag sin kode og sett tilstanden til oppdaget, søket går på følgende kriterier:
        ///   1 - bokstav stemmer
        ///   2 - koordinatene er innenfor rekkevidde
        ///   3 - tilstanden ikke allerede er satt til oppdaget
        /// </summary>
        /// <returns>true hvis alle kriterier er oppfylt</returns>
        public bool SettKodeTilstandTilOppdaget(string lagId, int postnummer, string kode, Koordinat koordinat)
        {
            var lag = _lagGameService.HentLagMedLagId(lagId);

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
                    _lagService.Oppdater(lag);
                    return true;
                default:
                    throw new AmbiguousMatchException("Flere koder funnet basert på kriteriene gitt");
            }
        }
    }
}