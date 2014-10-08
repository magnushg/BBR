using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using BouvetCodeCamp.Domene;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester.Interfaces;

namespace BouvetCodeCamp.DomeneTjenester
{
    using System.Threading.Tasks;

    public class PostService : IPostService
    {
        private readonly ILagService _lagService;
        private readonly IKoordinatVerifier koordinatVerifier;

        private readonly IRepository<Post> postRepository;

        public PostService(ILagService lagService, 
            IKoordinatVerifier koordinatVerifier,
            IRepository<Post> postRepository)
        {
            _lagService = lagService;
            this.koordinatVerifier = koordinatVerifier;
            this.postRepository = postRepository;
        }

        public IEnumerable<LagPost> HentOppdagedePoster(string lagId)
        {
            return _lagService.HentLagMedLagId(lagId).Poster.Where(post => post.PostTilstand == PostTilstand.Oppdaget);
        }

        public IEnumerable<LagPost> HentAllePosterForLag(string lagId)
        {
            var lag = _lagService.HentLagMedLagId(lagId);

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
            var lag = _lagService.HentLagMedLagId(lagId);

            var kandidater = lag.Poster.Where(k => k.Kode.Equals(kode, StringComparison.CurrentCultureIgnoreCase) 
                && k.Nummer == postnummer
                && koordinatVerifier.KoordinaterErNærHverandre(k.Posisjon, koordinat)
                && k.PostTilstand.Equals(PostTilstand.Ukjent)).ToList();

            switch (kandidater.Count())
            {
                case 0:
                    return false;
                case 1:
                    kandidater.First().PostTilstand = PostTilstand.Oppdaget;
                    lag.Poeng += 300000;
                    _lagService.Oppdater(lag);
                    return true;
                default:
                    throw new AmbiguousMatchException("Flere koder funnet basert på kriteriene gitt");
            }
        }

        public IEnumerable<Post> HentAlle()
        {
            return postRepository.HentAlle();
        }

        public Post Hent(string id)
        {
            return postRepository.Hent(id);
        }

        public async Task Opprett(Post document)
        {
            await postRepository.Opprett(document);
        }

        public async Task Oppdater(Post post)
        {
            await postRepository.Oppdater(post);
        }

        public async Task SlettAlle()
        {
            await postRepository.SlettAlle();
        }

        public async Task Slett(Post post)
        {
            await postRepository.Slett(post);
        }
    }
}