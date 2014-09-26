using System.Threading.Tasks;
using BouvetCodeCamp.Domene.Entiteter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace BouvetCodeCamp.Integrasjonstester.DataAksess
{
    using BouvetCodeCamp.Infrastruktur.DataAksess;
    using BouvetCodeCamp.Infrastruktur.DataAksess.Repositories;

    using FizzWare.NBuilder;

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

            var lagretPost = repo.Hent(documentId);

            var alle = repo.HentAlle();

            lagretPost.DocumentId.ShouldNotBeEmpty();
            lagretPost.Latitude.ShouldEqual(postSomSkalLagres.Latitude);
            lagretPost.Longitude.ShouldEqual(postSomSkalLagres.Longitude);
            lagretPost.Beskrivelse.ShouldEqual(postSomSkalLagres.Beskrivelse);
        }

        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task SlettAlle_HarEnPost_HarIngenPoster()
        {
            // Arrange
            var repository = OpprettRepository();

            var post = Builder<Post>.CreateNew()
                .Build();

            await repository.Opprett(post);

            // Act
            await repository.SlettAlle();

            // Assert
            var allePoster = repository.HentAlle();

            allePoster.ShouldBeEmpty();
        }

        private PostRepository OpprettRepository()
        {
            return new PostRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));
        }
    }
}