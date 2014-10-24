using System.Collections.Generic;
using System.Linq;
using BouvetCodeCamp.Domene.Entiteter;

namespace BouvetCodeCamp.SpillOppretter
{
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Data;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Data.Repositories;

    using BouvetCodeCamp.CrossCutting;

    public class KartdataLagring
    {
        private readonly PostRepository _postRepository;
        public KartdataLagring()
        {
            var log = Log4NetLogger.HentLogger(typeof(KartdataLagring));
            _postRepository = new PostRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()), log);
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