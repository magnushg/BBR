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
                Posisjon = new Koordinat
                {
                    Latitude = "59.666",
                    Longitude = "23.555"
                },
                Beskrivelse = "Blabla", 
                Navn = "Testepost",
                Kilde = "Nokia 3110",
            };

            var documentId = repo.Opprett(postSomSkalLagres).Result;

            var lagretPost = repo.Hent(documentId);

            var alle = repo.HentAlle();

            lagretPost.DocumentId.ShouldNotBeEmpty();
            lagretPost.Posisjon.Latitude.ShouldEqual(postSomSkalLagres.Posisjon.Latitude);
            lagretPost.Posisjon.Longitude.ShouldEqual(postSomSkalLagres.Posisjon.Longitude);
            lagretPost.Beskrivelse.ShouldEqual(postSomSkalLagres.Beskrivelse);
        }


        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Slett_HarFlerePosterSomSlettes_GirIngenPoster()
        {
            // Arrange
            var repository = OpprettRepository();

            var postSomSkalOpprettes = Builder<Post>.CreateListOfSize(5).All().Build();

            foreach (var post in postSomSkalOpprettes)
            {
                await repository.Opprett(post);
            }

            // Act
            var allePosterForSletting = repository.HentAlle();

            foreach (var post in allePosterForSletting)
            {
                await repository.Slett(post);
            }

            // Assert
            var ingenPoster = repository.HentAlle();

            ingenPoster.ShouldBeEmpty();
        }
        
        private PostRepository OpprettRepository()
        {
            return new PostRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));
        }
    }
}