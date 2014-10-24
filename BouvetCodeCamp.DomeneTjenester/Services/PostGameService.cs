namespace BouvetCodeCamp.DomeneTjenester.Services
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Domene;
using Domene.Entiteter;
using Interfaces;

    public class PostGameService : IPostGameService
{
        private readonly IKoordinatVerifier _koordinatVerifier;

        private readonly IService<Lag> lagService;

        public PostGameService(
            IKoordinatVerifier koordinatVerifier,
            IService<Lag> lagService)
        {
            _koordinatVerifier = koordinatVerifier;
            this.lagService = lagService;
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
        public HendelseType SettKodeTilstandTilOppdaget(Lag lag, int postnummer, string kode, Koordinat koordinat)
        {
            List<LagPost> postMatch = 
                lag.Poster.Where(
                        k => k.Kode.Equals(kode, StringComparison.CurrentCultureIgnoreCase) && 
                        k.Nummer == postnummer && 
                        //_koordinatVerifier.KoordinaterErNærHverandre(k.Posisjon, koordinat) && 
                        k.PostTilstand.Equals(PostTilstand.Ukjent))
                .ToList();

            switch (postMatch.Count())
            {
                case 0:
                    return HendelseType.RegistrertKodeMislykket;
                case 1:
                    postMatch.Single().PostTilstand = PostTilstand.Oppdaget;
                    return HendelseType.RegistrertKodeSuksess;
                default:
                    throw new AmbiguousMatchException("Flere koder funnet basert på kriteriene gitt");
            }
        }
    }
}