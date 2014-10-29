namespace Bouvet.BouvetBattleRoyale.Integrasjonstester.DataAksess
{
    using System.Threading.Tasks;

    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Tjenester.Interfaces;

    using BouvetCodeCamp.Integrasjonstester;
    using BouvetCodeCamp.Integrasjonstester.DataAksess;

    using FizzWare.NBuilder;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Should;

    [TestClass]
    public class PostRepositoryIntegrasjonstester : BaseRepositoryIntegrasjonstest
    {
        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Kan_opprette_repository_og_legge_til_en_post()
        {
            var repo = Resolve<IRepository<Post>>();

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
            var repository = Resolve<IRepository<Post>>();

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
    }
}