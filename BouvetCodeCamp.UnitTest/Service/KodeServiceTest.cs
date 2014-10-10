using System.Collections.Generic;
using BouvetCodeCamp.Domene;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester;
using BouvetCodeCamp.DomeneTjenester.Interfaces;
using Moq;
using NUnit.Framework;
using Should;

namespace BouvetCodeCamp.UnitTest.Service
{
    [TestFixture]
    class KodeServiceTest
    {
        private IPostService _postService;
        private readonly Mock<ILagService> _lagServiceMock = new Mock<ILagService>();
        private readonly Mock<IKoordinatVerifier> _coordinatMock = new Mock<IKoordinatVerifier>();
        private readonly Mock<IRepository<Post>> _postRepositoryMock = new Mock<IRepository<Post>>();

        [SetUp]
        public void Setup()
        {
            _postService = new PostService(_lagServiceMock.Object, _coordinatMock.Object, _postRepositoryMock.Object);
        }

        [Test]
        public void SettPostTilstandTilOppdaget_GyldigKode_BlirFlaggetOgReturnererTrue()
        {
            // Arrange
            var koordinat = new Koordinat("0", "0");
            var innsendtKode = new LagPost {Kode = "a", Nummer = 5, Posisjon = koordinat, PostTilstand = PostTilstand.Ukjent};
            var lag = new Lag() { Poster = new List<LagPost> { innsendtKode }};

            _lagServiceMock.Setup(x => x.HentLagMedLagId(It.IsAny<string>())).Returns(lag);
            _coordinatMock.Setup(x => x.KoordinaterErNærHverandre(It.IsAny<Koordinat>(), It.IsAny<Koordinat>())).Returns(true);

            // Act
            var resultat = _postService.SettKodeTilstandTilOppdaget("1", innsendtKode.Nummer, innsendtKode.Kode, innsendtKode.Posisjon);

            // Assert
            resultat.ShouldEqual(HendelseType.RegistrertKodeSuksess);
            innsendtKode.PostTilstand.ShouldEqual(PostTilstand.Oppdaget);
        }

        [Test]
        public void SettKodeTilstandTilOppdaget_Oppdaget_ReturnsFalse()
        {
            // Arrange
            var koordinat = new Koordinat("0", "0");
            var innsendtKode = new LagPost { Kode = "a", Nummer = 5, Posisjon = koordinat, PostTilstand = PostTilstand.Oppdaget };
            var lag = new Lag() { Poster = new List<LagPost> { innsendtKode }};

            _lagServiceMock.Setup(x => x.HentLagMedLagId(It.IsAny<string>())).Returns(lag);
            _coordinatMock.Setup(x => x.KoordinaterErNærHverandre(It.IsAny<Koordinat>(), It.IsAny<Koordinat>())).Returns(true);

            // Act
            var resultat = _postService.SettKodeTilstandTilOppdaget("1", innsendtKode.Nummer, innsendtKode.Kode, innsendtKode.Posisjon);

            // Assert
            resultat.ShouldEqual(HendelseType.RegistrertKodeMislykket);
        }

        [Test]
        public void SettKodeTilstandTilOppdaget_ForskjelligCasingPåKode_KodeErGodkjent()
        {
            // Arrange
            var koordinat = new Koordinat("0", "0");
            var innsendtKode = new LagPost { Kode = "a", Nummer = 5, Posisjon = koordinat, PostTilstand = PostTilstand.Ukjent };

            var lag = new Lag() { Poster = new List<LagPost> { innsendtKode }};

            _lagServiceMock.Setup(x => x.HentLagMedLagId(It.IsAny<string>())).Returns(lag);
            _coordinatMock.Setup(x => x.KoordinaterErNærHverandre(It.IsAny<Koordinat>(), It.IsAny<Koordinat>())).Returns(true);

            // Act
            var resultat = _postService.SettKodeTilstandTilOppdaget("1", innsendtKode.Nummer, innsendtKode.Kode, innsendtKode.Posisjon);

            // Assert
            resultat.ShouldEqual(HendelseType.RegistrertKodeSuksess);
        }

        [Test]
        public void SettKodeTilstandTilOppdaget_UgyldigKoordinat_ReturnsFalse()
        {
            // Arrage
            var koordinat = new Koordinat("0", "0");
            var innsendtKode = new LagPost { Kode = "a", Nummer = 5, Posisjon = koordinat, PostTilstand = PostTilstand.Ukjent };

            var lag = new Lag { Poster = new List<LagPost> { innsendtKode }};

            _lagServiceMock.Setup(x => x.HentLagMedLagId(It.IsAny<string>())).Returns(lag);
            _coordinatMock.Setup(x => x.KoordinaterErNærHverandre(It.IsAny<Koordinat>(), It.IsAny<Koordinat>())).Returns(false);

            // Act
            var resultat = _postService.SettKodeTilstandTilOppdaget("1", innsendtKode.Nummer, innsendtKode.Kode, innsendtKode.Posisjon);

            // Assert
            resultat.ShouldEqual(HendelseType.RegistrertKodeMislykket);
        }

        [Test]
        [ExpectedException]
        public void SettKodeTilstandTilOppdaget_FlereTilsvarendeKoderFunnet_KasterException()
        {
            // Arrange
            var koordinat = new Koordinat("0", "0");
            var innsendtKode = new LagPost { Kode = "a", Nummer = 5, Posisjon = koordinat, PostTilstand = PostTilstand.Ukjent };
            var identiskKode = new LagPost { Kode = "a", Nummer = 5, Posisjon = koordinat, PostTilstand = PostTilstand.Ukjent };

            var lag = new Lag() { Poster = new List<LagPost> { innsendtKode, identiskKode } };

            _lagServiceMock.Setup(x => x.HentLagMedLagId(It.IsAny<string>())).Returns(lag);
            _coordinatMock.Setup(x => x.KoordinaterErNærHverandre(It.IsAny<Koordinat>(), It.IsAny<Koordinat>())).Returns(true);

            // Act
            var resultat = _postService.SettKodeTilstandTilOppdaget("1", innsendtKode.Nummer, innsendtKode.Kode, innsendtKode.Posisjon);
        }
    }
}
