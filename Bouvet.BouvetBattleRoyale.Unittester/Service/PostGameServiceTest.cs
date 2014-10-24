namespace Bouvet.BouvetBattleRoyale.Unittests.Service
{
    using System.Collections.Generic;

    using Bouvet.BouvetBattleRoyale.Domene;
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;

    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;
    using BouvetCodeCamp.DomeneTjenester.Services;

    using Moq;

    using NUnit.Framework;

    using Should;

    [TestFixture]
    class PostGameServiceTest
    {
        private IPostGameService _postGameService;
        private readonly Mock<IKoordinatVerifier> _coordinatMock = new Mock<IKoordinatVerifier>();
        private readonly Mock<IService<Lag>> _lagServiceMock = new Mock<IService<Lag>>();

        [SetUp]
        public void Setup()
        {
            _postGameService = new PostGameService(
                _coordinatMock.Object,
                _lagServiceMock.Object);
        }

        [Test]
        public void SettPostTilstandTilOppdaget_GyldigKode_BlirFlaggetOgReturnererTrue()
        {
            // Arrange
            var koordinat = new Koordinat("0", "0");
            var innsendtKode = new LagPost {Kode = "a", Nummer = 5, Posisjon = koordinat, PostTilstand = PostTilstand.Ukjent};
            var lag = new Lag() { Poster = new List<LagPost> { innsendtKode }};

            _coordinatMock.Setup(x => x.KoordinaterErNærHverandre(It.IsAny<Koordinat>(), It.IsAny<Koordinat>())).Returns(true);
            _lagServiceMock.Setup(x => x.Oppdater(It.IsAny<Lag>()));

            // Act
            var resultat = _postGameService.SettKodeTilstandTilOppdaget(lag, innsendtKode.Nummer, innsendtKode.Kode, innsendtKode.Posisjon);

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

            _coordinatMock.Setup(x => x.KoordinaterErNærHverandre(It.IsAny<Koordinat>(), It.IsAny<Koordinat>())).Returns(true);

            // Act
            var resultat = _postGameService.SettKodeTilstandTilOppdaget(lag, innsendtKode.Nummer, innsendtKode.Kode, innsendtKode.Posisjon);

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

            _coordinatMock.Setup(x => x.KoordinaterErNærHverandre(It.IsAny<Koordinat>(), It.IsAny<Koordinat>())).Returns(true);
            _lagServiceMock.Setup(x => x.Oppdater(It.IsAny<Lag>()));

            // Act
            var resultat = _postGameService.SettKodeTilstandTilOppdaget(lag, innsendtKode.Nummer, innsendtKode.Kode, innsendtKode.Posisjon);

            // Assert
            resultat.ShouldEqual(HendelseType.RegistrertKodeSuksess);
        }

        [Test]
        [Ignore("Vi tester ikke lengre dette")]
        public void SettKodeTilstandTilOppdaget_UgyldigKoordinat_ReturnsFalse()
        {
            // Arrage
            var koordinat = new Koordinat("0", "0");
            var innsendtKode = new LagPost { Kode = "a", Nummer = 5, Posisjon = koordinat, PostTilstand = PostTilstand.Ukjent };

            var lag = new Lag { Poster = new List<LagPost> { innsendtKode }};

            _coordinatMock.Setup(x => x.KoordinaterErNærHverandre(It.IsAny<Koordinat>(), It.IsAny<Koordinat>())).Returns(false);

            // Act
            var resultat = _postGameService.SettKodeTilstandTilOppdaget(lag, innsendtKode.Nummer, innsendtKode.Kode, innsendtKode.Posisjon);

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

            _coordinatMock.Setup(x => x.KoordinaterErNærHverandre(It.IsAny<Koordinat>(), It.IsAny<Koordinat>())).Returns(true);

            // Act
            var resultat = _postGameService.SettKodeTilstandTilOppdaget(lag, innsendtKode.Nummer, innsendtKode.Kode, innsendtKode.Posisjon);
        }
    }
}
