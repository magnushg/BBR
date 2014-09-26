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
        private IKodeService _kodeService;
        private readonly Mock<ILagService> _lagServiceMock = new Mock<ILagService>();
        private readonly Mock<ICoordinateVerifier> _coordinatMock = new Mock<ICoordinateVerifier>();

        [SetUp]
        public void Setup()
        {
            _kodeService = new KodeService(_lagServiceMock.Object, _coordinatMock.Object);
        }

        [Test]
        public void SettKodeTilstandTilOppdaget_GyldigKode_BlirFlaggetOgReturnererTrue()
        {
            // Arrange
            var koordinat = new Coordinate("0", "0");
            var innsendtKode = new Kode() {Bokstav = "a", Posisjon = koordinat, PosisjonTilstand = PosisjonTilstand.Ukjent};
            var lag = new Lag() { Koder = new List<Kode> { innsendtKode }};

            _lagServiceMock.Setup(x => x.HentLag(It.IsAny<string>())).Returns(lag);
            _coordinatMock.Setup(x => x.CoordinatesAreInProximity(It.IsAny<Coordinate>(), It.IsAny<Coordinate>())).Returns(true);

            // Act
            var resultat = _kodeService.SettKodeTilstandTilOppdaget("1", innsendtKode.Bokstav, innsendtKode.Posisjon);

            // Assert
            resultat.ShouldBeTrue();
            innsendtKode.PosisjonTilstand.ShouldEqual(PosisjonTilstand.Oppdaget);
        }

        [Test]
        public void SettKodeTilstandTilOppdaget_Oppdaget_ReturnsFalse()
        {
            // Arrange
            var koordinat = new Coordinate("0", "0");
            var innsendtKode = new Kode() {Bokstav = "a", Posisjon = koordinat, PosisjonTilstand = PosisjonTilstand.Oppdaget};
            var lag = new Lag() { Koder = new List<Kode> { innsendtKode }};

            _lagServiceMock.Setup(x => x.HentLag(It.IsAny<string>())).Returns(lag);
            _coordinatMock.Setup(x => x.CoordinatesAreInProximity(It.IsAny<Coordinate>(), It.IsAny<Coordinate>())).Returns(true);

            // Act
            var resultat = _kodeService.SettKodeTilstandTilOppdaget("1", innsendtKode.Bokstav, innsendtKode.Posisjon);

            // Assert
            resultat.ShouldBeFalse();
        }

        [Test]
        public void SettKodeTilstandTilOppdaget_ForskjelligCasingPåKode_KodeErGodkjent()
        {
            // Arrange
            var koordinat = new Coordinate("0", "0");
            var innsendtKode = new Kode() {Bokstav = "a", Posisjon = koordinat, PosisjonTilstand = PosisjonTilstand.Ukjent};
          
            var lag = new Lag() { Koder = new List<Kode> { innsendtKode }};

            _lagServiceMock.Setup(x => x.HentLag(It.IsAny<string>())).Returns(lag);
            _coordinatMock.Setup(x => x.CoordinatesAreInProximity(It.IsAny<Coordinate>(), It.IsAny<Coordinate>())).Returns(true);

            // Act
            var resultat = _kodeService.SettKodeTilstandTilOppdaget("1", "A", innsendtKode.Posisjon);

            // Assert
            resultat.ShouldBeTrue();
        }

        [Test]
        public void SettKodeTilstandTilOppdaget_UgyldigKoordinat_ReturnsFalse()
        {
            // Arrage
            var koordinat = new Coordinate("0", "0");
            var innsendtKode = new Kode() {Bokstav = "a", Posisjon = koordinat, PosisjonTilstand = PosisjonTilstand.Ukjent};

            var lag = new Lag() { Koder = new List<Kode> { innsendtKode }};

            _lagServiceMock.Setup(x => x.HentLag(It.IsAny<string>())).Returns(lag);
            _coordinatMock.Setup(x => x.CoordinatesAreInProximity(It.IsAny<Coordinate>(), It.IsAny<Coordinate>())).Returns(false);

            // Act
            var resultat = _kodeService.SettKodeTilstandTilOppdaget("1", innsendtKode.Bokstav, innsendtKode.Posisjon);

            // Assert
            resultat.ShouldBeFalse();
        }

        [Test]
        [ExpectedException]
        public void SettKodeTilstandTilOppdaget_FlereTilsvarendeKoderFunnet_KasterException()
        {
            // Arrange
            var koordinat = new Coordinate("0", "0");
            var innsendtKode = new Kode() {Bokstav = "a", Posisjon = koordinat, PosisjonTilstand = PosisjonTilstand.Ukjent};
            var identiskKode = new Kode() {Bokstav = "a", Posisjon = koordinat, PosisjonTilstand = PosisjonTilstand.Ukjent};

            var lag = new Lag() { Koder = new List<Kode> { innsendtKode, identiskKode } };

            _lagServiceMock.Setup(x => x.HentLag(It.IsAny<string>())).Returns(lag);
            _coordinatMock.Setup(x => x.CoordinatesAreInProximity(It.IsAny<Coordinate>(), It.IsAny<Coordinate>())).Returns(true);

            // Act
            _kodeService.SettKodeTilstandTilOppdaget("1", innsendtKode.Bokstav, innsendtKode.Posisjon);
        }
    }
}
