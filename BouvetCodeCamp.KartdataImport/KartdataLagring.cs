using System.Collections.Generic;
using System.Linq;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.Infrastruktur.DataAksess;
using BouvetCodeCamp.Infrastruktur.DataAksess.Repositories;

namespace BouvetCodeCamp.KartdataImport
{
    public class KartdataLagring
    {
        private readonly PostRepository _postRepository;
        public KartdataLagring()
        {
            _postRepository = new PostRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));
        }

        public void LagreKartdata(IEnumerable<Domene.Entiteter.Post> poster)
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