using System.Collections.Generic;
using BouvetCodeCamp.Felles;
using BouvetCodeCamp.Felles.Entiteter;
using BouvetCodeCamp.Felles.Interfaces;
using BouvetCodeCamp.Service.Interfaces;
using BouvetCodeCamp.Service.Services;
using Moq;
using NUnit.Framework;

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
        public async void SettKodeTilstandTilOppdaget_GyldigKode_BlirFlaggetOgReturnererTrue()
        {
            var koordinat = new Coordinate("0", "0");
            var innsendtKode = new Kode() {Bokstav = "a", Gps = koordinat, PosisjonTilstand = PosisjonTilstand.Ukjent};
            var lag = new Lag() { Koder = new List<Kode> { innsendtKode }};

            _lagServiceMock.Setup(x => x.HentLag(It.IsAny<string>())).ReturnsAsync(lag);
            _coordinatMock.Setup(x => x.CoordinateSAreInProximity(It.IsAny<Coordinate>(), It.IsAny<Coordinate>())).Returns(true);

            var resultat = await _kodeService.SettKodeTilstandTilOppdaget("1", innsendtKode.Bokstav, innsendtKode.Gps);

            Assert.IsTrue(resultat);
            Assert.AreEqual(innsendtKode.PosisjonTilstand, PosisjonTilstand.Oppdaget);
        }

        [Test]
        public async void SettKodeTilstandTilOppdaget_Oppdaget_ReturnsFalse()
        {
            var koordinat = new Coordinate("0", "0");
            var innsendtKode = new Kode() {Bokstav = "a", Gps = koordinat, PosisjonTilstand = PosisjonTilstand.Oppdaget};
            var lag = new Lag() { Koder = new List<Kode> { innsendtKode }};

            _lagServiceMock.Setup(x => x.HentLag(It.IsAny<string>())).ReturnsAsync(lag);
            _coordinatMock.Setup(x => x.CoordinateSAreInProximity(It.IsAny<Coordinate>(), It.IsAny<Coordinate>())).Returns(true);

            var resultat = await _kodeService.SettKodeTilstandTilOppdaget("1", innsendtKode.Bokstav, innsendtKode.Gps);

            Assert.IsFalse(resultat);
        }

        [Test]
        public async void SettKodeTilstandTilOppdaget_IgnorerCapsLockIKode()
        {
            var koordinat = new Coordinate("0", "0");
            var innsendtKode = new Kode() {Bokstav = "a", Gps = koordinat, PosisjonTilstand = PosisjonTilstand.Ukjent};
            var databaseKode = new Kode() {Bokstav = "A", Gps = koordinat, PosisjonTilstand = PosisjonTilstand.Ukjent};

            var lag = new Lag() { Koder = new List<Kode> { innsendtKode }};

            _lagServiceMock.Setup(x => x.HentLag(It.IsAny<string>())).ReturnsAsync(lag);
            _coordinatMock.Setup(x => x.CoordinateSAreInProximity(It.IsAny<Coordinate>(), It.IsAny<Coordinate>())).Returns(true);

            var resultat = await _kodeService.SettKodeTilstandTilOppdaget("1", innsendtKode.Bokstav, innsendtKode.Gps);

            Assert.IsTrue(resultat);
        }

        [Test]
        public async void SettKodeTilstandTilOppdaget_UgyldigKoordinat_ReturnsFalse()
        {
            var koordinat = new Coordinate("0", "0");
            var innsendtKode = new Kode() {Bokstav = "a", Gps = koordinat, PosisjonTilstand = PosisjonTilstand.Ukjent};

            var lag = new Lag() { Koder = new List<Kode> { innsendtKode }};

            _lagServiceMock.Setup(x => x.HentLag(It.IsAny<string>())).ReturnsAsync(lag);
            _coordinatMock.Setup(x => x.CoordinateSAreInProximity(It.IsAny<Coordinate>(), It.IsAny<Coordinate>())).Returns(false);

            var resultat = await _kodeService.SettKodeTilstandTilOppdaget("1", innsendtKode.Bokstav, innsendtKode.Gps);

            Assert.IsFalse(resultat);
        }

        [Test]
        [ExpectedException]
        public async void SettKodeTilstandTilOppdaget_FlereTilsvarendeKoderFunnet_KasterException()
        {
            var koordinat = new Coordinate("0", "0");
            var innsendtKode = new Kode() {Bokstav = "a", Gps = koordinat, PosisjonTilstand = PosisjonTilstand.Ukjent};
            var identiskKode = new Kode() {Bokstav = "a", Gps = koordinat, PosisjonTilstand = PosisjonTilstand.Ukjent};

            var lag = new Lag() { Koder = new List<Kode> { innsendtKode, identiskKode } };

            _lagServiceMock.Setup(x => x.HentLag(It.IsAny<string>())).ReturnsAsync(lag);
            _coordinatMock.Setup(x => x.CoordinateSAreInProximity(It.IsAny<Coordinate>(), It.IsAny<Coordinate>())).Returns(true);

            var resultat = await _kodeService.SettKodeTilstandTilOppdaget("1", innsendtKode.Bokstav, innsendtKode.Gps);
        }
    }
}
