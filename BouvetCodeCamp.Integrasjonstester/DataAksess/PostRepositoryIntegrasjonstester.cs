using System.Threading.Tasks;
using BouvetCodeCamp.Dataaksess;
using BouvetCodeCamp.Domene.Entiteter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace BouvetCodeCamp.Integrasjonstester.DataAksess
{
    using System;

    using BouvetCodeCamp.Infrastruktur;
    using BouvetCodeCamp.Infrastruktur.Repositories;

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
                GpsPunktId = Guid.NewGuid().ToString(),
                Latitude = "59.666",
                Longitude = "23.555",
                Kommentar = "Blabla", 
                OpprettetDato = DateTime.Now
            };

            var documentId = repo.Opprett(postSomSkalLagres).Result;

            var lagretPost = await repo.Hent(documentId);

            var alle = await repo.HentAlle();

            lagretPost.DocumentId.ShouldNotBeEmpty();
            lagretPost.Latitude.ShouldEqual(postSomSkalLagres.Latitude);
            lagretPost.Longitude.ShouldEqual(postSomSkalLagres.Longitude);
            lagretPost.Kommentar.ShouldEqual(postSomSkalLagres.Kommentar);
        }

        private PostRepository OpprettRepository()
        {
            return new PostRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));
        }
    }

    
}
