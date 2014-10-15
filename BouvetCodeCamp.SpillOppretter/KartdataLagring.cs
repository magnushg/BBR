using System.Collections.Generic;
using System.Linq;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.Infrastruktur.DataAksess;
using BouvetCodeCamp.Infrastruktur.DataAksess.Repositories;

namespace BouvetCodeCamp.SpillOppretter
{
    public class KartdataLagring
    {
        private readonly PostRepository _postRepository;
        public KartdataLagring()
        {
            _postRepository = new PostRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));
        }

        public IEnumerable<Post> LagreKartdata(IEnumerable<Post> poster)
        {
            poster.ToList().ForEach(async x =>
            {
                await _postRepository.Opprett(x);
            });

            return poster.ToList();
        }

        public async void SlettAlleKartdata()
        {
            var allePoster = _postRepository.HentAlle();

            foreach (var post in allePoster)
            {
                await _postRepository.Slett(post);
            }
        }
    }
}