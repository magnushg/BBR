using System;
using BouvetCodeCamp.Felles;
using BouvetCodeCamp.Felles.Entiteter;
using BouvetCodeCamp.Felles.Interfaces;
using NUnit.Framework;

namespace BouvetCodeCamp.UnitTest.Felles
{
    [TestFixture]
    public class CoordinateVerifierTest
    {
        private ICoordinateVerifier _coordinateVerifier;

        [SetUp]
        public void Setup()
        {
            _coordinateVerifier = new CoordinateVerifier();
        }

        [Test]
        public void CoordinateSAreInProximity_PerfectMatch_ReturnsTrue()
        {
            Coordinate first = new Coordinate("45.45", "45.45"),
                second = new Coordinate("45.45", "45.45");

            Assert.IsTrue(_coordinateVerifier.CoordinateSAreInProximity(first, second));
        }

        [Test]
        public void CoordinateSAreInProximity_JustWithinThreshold_ReturnsTrue()
        {
            Coordinate first = new Coordinate("10", "10");
            var latThreshold = CoordinateVerifier.LatProximityThreshold;
            var longThreshold = CoordinateVerifier.LongProximityThreshold;

            var lngd =  (10 + longThreshold);
            var latd =  (10 + latThreshold);

            var second = new Coordinate(lngd.ToString(), latd.ToString());

            Assert.IsTrue(_coordinateVerifier.CoordinateSAreInProximity(first, second));
        }

        [Test]
        public void CoordinateSAreInProximity_LatOutsideThreshold_ReturnsFalse()
        {
            Coordinate first = new Coordinate("10", "10");
            var latThreshold = CoordinateVerifier.LatProximityThreshold;
            var longThreshold = CoordinateVerifier.LongProximityThreshold;

            var lngd = (10 + longThreshold);
            var latd = (10 + latThreshold) + 1;

            var second = new Coordinate(lngd.ToString(), latd.ToString());

            Assert.IsFalse(_coordinateVerifier.CoordinateSAreInProximity(first, second));
        }

        [Test]
        public void CoordinateSAreInProximity_LongOutsideThreshold_ReturnsFalse()
        {
            Coordinate first = new Coordinate("10", "10");
            var latThreshold = CoordinateVerifier.LatProximityThreshold;
            var longThreshold = CoordinateVerifier.LongProximityThreshold;

            var lngd = (10 + longThreshold) + 1;
            var latd = (10 + latThreshold);

            var second = new Coordinate(lngd.ToString(), latd.ToString());

            Assert.IsFalse(_coordinateVerifier.CoordinateSAreInProximity(first, second));
        }

        [Test]
        public void IsStringValidCoordinate_ValidPositiveNoWhitespaceCoord_ReturnsTrue()
        {
            string c ="40,40";

            Assert.IsTrue(_coordinateVerifier.IsStringValidCoordinate(c));
        }

        [Test]
        public void IsStringValidCoordinate_ValidPositiveWithWhitespaceCoord_ReturnsTrue()
        {
            string c ="40, 40";

            Assert.IsTrue(_coordinateVerifier.IsStringValidCoordinate(c));
        }

        [Test]
        public void IsStringValidCoordinate_ValidSecondNegativeWithWhitespaceCoord_ReturnsTrue()
        {
            string c = "40, -40";

            Assert.IsTrue(_coordinateVerifier.IsStringValidCoordinate(c));
        }

        [Test]
        public void IsStringValidCoordinate_ValidFirstNegativeWithWhitespaceCoord_ReturnsTrue()
        {
            string c = "-40, 40";

            Assert.IsTrue(_coordinateVerifier.IsStringValidCoordinate(c));
        }

        [Test]
        public void IsStringValidCoordinate_ValidSecondNegativeNoWhitespaceCoord_ReturnsTrue()
        {
            string c = "40,-40";

            Assert.IsTrue(_coordinateVerifier.IsStringValidCoordinate(c));
        }

        [Test]
        public void IsStringValidCoordinate_ValidFirstNegativeWhitespaceCoord_ReturnsTrue()
        {
            string c = "-40,40";

            Assert.IsTrue(_coordinateVerifier.IsStringValidCoordinate(c));
        }

        [Test]
        //ble lei av å skrive unit test for hvert case
        public void IsStringValidCoordinate_BadMatches_ReturnsFalse()
        {
            string c1 = "-90., -180",
                c2 = "+90.1, -100.111",
                c3 = "-91, 123.456",
                c4 = "045, 180",
                c5 = "90,",
                c6 = ",45";

            Assert.IsFalse(_coordinateVerifier.IsStringValidCoordinate(c1));
            Assert.IsFalse(_coordinateVerifier.IsStringValidCoordinate(c2));
            Assert.IsFalse(_coordinateVerifier.IsStringValidCoordinate(c3));
            Assert.IsFalse(_coordinateVerifier.IsStringValidCoordinate(c4));
        }

        [Test]
        public void IsStringValidCoordinate_LatitudeOutOfRange_ReturnsFalse()
        {
            string under = "45.0, -180.1", over = "45.0, 180.1";

            Assert.IsFalse(_coordinateVerifier.IsStringValidCoordinate(under));
            Assert.IsFalse(_coordinateVerifier.IsStringValidCoordinate(over));
        }

        [Test]
        public void IsStringValidCoordinate_LongitudeOfRange_ReturnsFalse()
        {
            string under = "90.1, -180.0", over = "90.1, 180.0";

            Assert.IsFalse(_coordinateVerifier.IsStringValidCoordinate(under));
            Assert.IsFalse(_coordinateVerifier.IsStringValidCoordinate(over));
        }
    }
}
