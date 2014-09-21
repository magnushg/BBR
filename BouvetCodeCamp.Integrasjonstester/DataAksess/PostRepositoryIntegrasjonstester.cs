using System.Threading.Tasks;
using BouvetCodeCamp.Dataaksess;
using BouvetCodeCamp.Dataaksess.Repositories;
using BouvetCodeCamp.Felles.Entiteter;
using BouvetCodeCamp.Felles.Konfigurasjon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace BouvetCodeCamp.Integrasjonstester.DataAksess
{
    [TestClass]
    public class PostRepositoryIntegrasjonstester : BaseRepositoryIntegrasjonstest
    {
        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Kan_opprette_repository_og_legge_til_en_post()
        {
            var repo = OpprettRepository();

            var postSomSkalLagres = new Post
            {
                Id = "12345",
                Latitude = "59.666",
                Longitude = "23.555",
                Kommentar = "Blabla"
            };

            var document = await repo.Opprett(postSomSkalLagres);

            var lagretPost = await repo.Hent(document.Id);

            var alle = await repo.HentAlle();

            lagretPost.Id.ShouldEqual(postSomSkalLagres.Id);

        }

        private PostRepository OpprettRepository()
        {
            return new PostRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));
        }
    }

    
}
