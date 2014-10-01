﻿using System.Collections.Generic;
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

        [SetUp]
        public void Setup()
        {
            _postService = new PostService(_lagServiceMock.Object, _coordinatMock.Object);
        }

        [Test]
        public void SettPostTilstandTilOppdaget_GyldigKode_BlirFlaggetOgReturnererTrue()
        {
            // Arrange
            var koordinat = new Koordinat("0", "0");
            var innsendtKode = new LagPost {Kode = "a", Posisjon = koordinat, PostTilstand = PostTilstand.Ukjent};
            var lag = new Lag() { Poster = new List<LagPost> { innsendtKode }};

            _lagServiceMock.Setup(x => x.HentLagMedLagId(It.IsAny<string>())).Returns(lag);
            _coordinatMock.Setup(x => x.KoordinaterErNærHverandre(It.IsAny<Koordinat>(), It.IsAny<Koordinat>())).Returns(true);

            // Act
            var resultat = _postService.SettKodeTilstandTilOppdaget("1", innsendtKode.Kode, innsendtKode.Posisjon);

            // Assert
            resultat.ShouldBeTrue();
            innsendtKode.PostTilstand.ShouldEqual(PostTilstand.Oppdaget);
        }

        [Test]
        public void SettKodeTilstandTilOppdaget_Oppdaget_ReturnsFalse()
        {
            // Arrange
            var koordinat = new Koordinat("0", "0");
            var innsendtKode = new LagPost {Kode = "a", Posisjon = koordinat, PostTilstand = PostTilstand.Oppdaget};
            var lag = new Lag() { Poster = new List<LagPost> { innsendtKode }};

            _lagServiceMock.Setup(x => x.HentLagMedLagId(It.IsAny<string>())).Returns(lag);
            _coordinatMock.Setup(x => x.KoordinaterErNærHverandre(It.IsAny<Koordinat>(), It.IsAny<Koordinat>())).Returns(true);

            // Act
            var resultat = _postService.SettKodeTilstandTilOppdaget("1", innsendtKode.Kode, innsendtKode.Posisjon);

            // Assert
            resultat.ShouldBeFalse();
        }

        [Test]
        public void SettKodeTilstandTilOppdaget_ForskjelligCasingPåKode_KodeErGodkjent()
        {
            // Arrange
            var koordinat = new Koordinat("0", "0");
            var innsendtKode = new LagPost {Kode = "a", Posisjon = koordinat, PostTilstand = PostTilstand.Ukjent};
          
            var lag = new Lag() { Poster = new List<LagPost> { innsendtKode }};

            _lagServiceMock.Setup(x => x.HentLagMedLagId(It.IsAny<string>())).Returns(lag);
            _coordinatMock.Setup(x => x.KoordinaterErNærHverandre(It.IsAny<Koordinat>(), It.IsAny<Koordinat>())).Returns(true);

            // Act
            var resultat = _postService.SettKodeTilstandTilOppdaget("1", "A", innsendtKode.Posisjon);

            // Assert
            resultat.ShouldBeTrue();
        }

        [Test]
        public void SettKodeTilstandTilOppdaget_UgyldigKoordinat_ReturnsFalse()
        {
            // Arrage
            var koordinat = new Koordinat("0", "0");
            var innsendtKode = new LagPost {Kode = "a", Posisjon = koordinat, PostTilstand = PostTilstand.Ukjent};

            var lag = new Lag { Poster = new List<LagPost> { innsendtKode }};

            _lagServiceMock.Setup(x => x.HentLagMedLagId(It.IsAny<string>())).Returns(lag);
            _coordinatMock.Setup(x => x.KoordinaterErNærHverandre(It.IsAny<Koordinat>(), It.IsAny<Koordinat>())).Returns(false);

            // Act
            var resultat = _postService.SettKodeTilstandTilOppdaget("1", innsendtKode.Kode, innsendtKode.Posisjon);

            // Assert
            resultat.ShouldBeFalse();
        }

        [Test]
        [ExpectedException]
        public void SettKodeTilstandTilOppdaget_FlereTilsvarendeKoderFunnet_KasterException()
        {
            // Arrange
            var koordinat = new Koordinat("0", "0");
            var innsendtKode = new LagPost {Kode = "a", Posisjon = koordinat, PostTilstand = PostTilstand.Ukjent};
            var identiskKode = new LagPost {Kode = "a", Posisjon = koordinat, PostTilstand = PostTilstand.Ukjent};

            var lag = new Lag() { Poster = new List<LagPost> { innsendtKode, identiskKode } };

            _lagServiceMock.Setup(x => x.HentLagMedLagId(It.IsAny<string>())).Returns(lag);
            _coordinatMock.Setup(x => x.KoordinaterErNærHverandre(It.IsAny<Koordinat>(), It.IsAny<Koordinat>())).Returns(true);

            // Act
            _postService.SettKodeTilstandTilOppdaget("1", innsendtKode.Kode, innsendtKode.Posisjon);
        }
    }
}
