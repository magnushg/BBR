using System.Threading.Tasks;
using BouvetCodeCamp.Domene.Entiteter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace BouvetCodeCamp.Integrasjonstester.DataAksess
{
    using System;

    using BouvetCodeCamp.Infrastruktur;
    using BouvetCodeCamp.Infrastruktur.DataAksess;
    using BouvetCodeCamp.Infrastruktur.DataAksess.Repositories;

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
                Latitude = "59.666",
                Longitude = "23.555",
                Beskrivelse = "Blabla", 
                Navn = "Testepost",
                Kilde = "Nokia 3110",
            };

            var documentId = repo.Opprett(postSomSkalLagres).Result;

            var lagretPost = await repo.Hent(documentId);

            var alle = await repo.HentAlle();

            lagretPost.DocumentId.ShouldNotBeEmpty();
            lagretPost.Latitude.ShouldEqual(postSomSkalLagres.Latitude);
            lagretPost.Longitude.ShouldEqual(postSomSkalLagres.Longitude);
            lagretPost.Beskrivelse.ShouldEqual(postSomSkalLagres.Beskrivelse);
        }

        private PostRepository OpprettRepository()
        {
            return new PostRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));
        }
    }
}