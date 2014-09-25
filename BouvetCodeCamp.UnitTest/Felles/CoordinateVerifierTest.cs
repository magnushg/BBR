using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester;
using BouvetCodeCamp.DomeneTjenester.Interfaces;
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

        ///
        /// CoordinateIsInPolygon
        ///
        [Test]
        public void CoordinateIsInPolygon_InsidePolygon_True()
        {
            Coordinate[] polygon = GetSquarePolygon();
            var point = new Coordinate(1.5, 1.5);

            Assert.IsTrue(_coordinateVerifier.CoordinateIsInPolygon(point, polygon));
        }

        [Test]
//        [Ignore]
        public void CoordinateIsInPolygon_LeftOfPolygon_False()
        {
            Coordinate[] polygon = GetSquarePolygon();
            var point = new Coordinate(0.5, 1);

            Assert.IsFalse(_coordinateVerifier.CoordinateIsInPolygon(point, polygon));
        }

        [Test]
        public void CoordinateIsInPolygon_RightOfPolygon_False()
        {
            Coordinate[] polygon = GetSquarePolygon();
            var point = new Coordinate(2.5, 1.5);

            Assert.IsFalse(_coordinateVerifier.CoordinateIsInPolygon(point, polygon));
        }

        [Test]
        public void CoordinateIsInPolygon_AbovePolygon_False()
        {
            Coordinate[] polygon = GetSquarePolygon();
            var point = new Coordinate(0.5, 1.5);

            Assert.IsFalse(_coordinateVerifier.CoordinateIsInPolygon(point, polygon));
        }

        [Test]
        public void CoordinateIsInPolygon_UnderPolygon_False()
        {
            Coordinate[] polygon = GetSquarePolygon();
            var point = new Coordinate(2.5, 1);

            Assert.IsFalse(_coordinateVerifier.CoordinateIsInPolygon(point, polygon));
        }

        [Test]
        public void CoordinateIsInPolygon_BottomRightCorner_True()
        {
            Coordinate[] polygon = GetSquarePolygon();
            var point = new Coordinate(2, 2);

            Assert.IsTrue(_coordinateVerifier.CoordinateIsInPolygon(point, polygon));
        }

        [Test]
        public void CoordinateIsInPolygon_BottomLeftCorner_False()
        {
            Coordinate[] polygon = GetSquarePolygon();
            var point = new Coordinate(1, 2);

            Assert.IsFalse(_coordinateVerifier.CoordinateIsInPolygon(point, polygon));
        }

        [Test]
        public void CoordinateIsInPolygon_TopLeftCorner_False()
        {
            Coordinate[] polygon = GetSquarePolygon();
            var point = new Coordinate(1, 1);

            Assert.IsFalse(_coordinateVerifier.CoordinateIsInPolygon(point, polygon));
        }

        [Test]
        public void CoordinateIsInPolygon_TopRightCorner_False()
        {
            Coordinate[] polygon = GetSquarePolygon();
            var point = new Coordinate(1, 2);

            Assert.IsFalse(_coordinateVerifier.CoordinateIsInPolygon(point, polygon));
        }

        [Test]
        public void CoordinateIsInPolygon_GetZiggyPolygon_Case1()
        {
            //            1             2         3
            //
            //     2      X------------------------X
            //            XX                     XXX
            //             XX                 XXX
            //     3        XX               XX
            //               XX            XX
            //                XX          XX
            //                 XX       XXX
            //     4            XX      XX   (p)
            //                  X        XX
            //                 XX         XX
            //     5          XX           XXX
            //               XX              XX
            //              X                 XX
            //             XX                  XX
            //     6      XXXXXXXXXXXXXXXXXXXXXXX
            Coordinate[] polygon = GetZiggyPolygon();
            var point = new Coordinate(4, 2.5);

            Assert.IsFalse(_coordinateVerifier.CoordinateIsInPolygon(point, polygon));
        }

        [Test]
        public void CoordinateIsInPolygon_GetZiggyPolygon_Case2()
        {
            //            1             2         3
            //
            //     2      X------------------------X
            //            XX                     XXX
            //             XX                 XXX
            //     3        XX         (p)   XX
            //               XX            XX
            //                XX          XX
            //                 XX       XXX
            //     4            XX      XX
            //                  X        XX
            //                 XX         XX
            //     5          XX           XXX
            //               XX              XX
            //              X                 XX
            //             XX                  XX
            //     6      XXXXXXXXXXXXXXXXXXXXXXX
            Coordinate[] polygon = GetZiggyPolygon();
            var point = new Coordinate(2, 3);

            Assert.IsTrue(_coordinateVerifier.CoordinateIsInPolygon(point, polygon));
        }

        [Test]
        public void CoordinateIsInPolygon_GetZiggyPolygon_Case3()
        {
            //            1             2         3
            //
            //     2      X----------(p)------------X
            //            XX                     XXX
            //             XX                 XXX
            //     3        XX               XX
            //               XX            XX
            //                XX          XX
            //                 XX       XXX
            //     4            XX      XX
            //                  X        XX
            //                 XX         XX
            //     5          XX           XXX
            //               XX              XX
            //              X                 XX
            //             XX                  XX
            //     6      XXXXXXXXXXXXXXXXXXXXXXX
            Coordinate[] polygon = GetZiggyPolygon();
            var point = new Coordinate(2, 2);

            Assert.IsFalse(_coordinateVerifier.CoordinateIsInPolygon(point, polygon));
        }

        [Test]
        public void CoordinateIsInPolygon_GetZiggyPolygon_Case4()
        {
            //            1             2         3
            //
            //     2      X-------------------------X
            //            XX                     XXX
            //             XX                 XXX
            //     3        XX               XX
            //               XX            XX
            //                XX          XX
            //                 XX       XXX
            //     4        (p) XX      XX
            //                  X        XX
            //                 XX         XX
            //     5          XX           XXX
            //               XX              XX
            //              X                 XX
            //             XX                  XX
            //     6      XXXXXXXXXXXXXXXXXXXXXXX
            Coordinate[] polygon = GetZiggyPolygon();
            var point = new Coordinate(1.49, 4);

            Assert.IsTrue(_coordinateVerifier.CoordinateIsInPolygon(point, polygon));
        }

        [Test]
        public void CoordinateIsInPolygon_GetZiggyPolygon_Case5()
        {
            //            1             2         3
            //
            //     2      X-------------------------X
            //            XX                     XXX
            //             XX                 XXX
            //     3        XX               XX
            //               XX            XX
            //                XX          XX
            //                 XX       XXX
            //     4            XX      XX
            //                  X        XX
            //                 XX         XX
            //     5          XX           XXX
            //               XX              XX
            //              X                 XX
            //             XX                  XX
            //     6      XXXXXXXXXXXXXXXXXXXXXXX
            //                     (p)
            Coordinate[] polygon = GetZiggyPolygon();
            var point = new Coordinate(1.7, 7);

            Assert.IsFalse(_coordinateVerifier.CoordinateIsInPolygon(point, polygon));
        }

        ///
        /// CoordinateSAreInProximity
        ///
        [Test]
        public void CoordinateSAreInProximity_PerfectMatch_ReturnsTrue()
        {
            Coordinate first = new Coordinate("45.45", "45.45"),
                second = new Coordinate("45.45", "45.45");

            Assert.IsTrue(_coordinateVerifier.CoordinatesAreInProximity(first, second));
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

            Assert.IsTrue(_coordinateVerifier.CoordinatesAreInProximity(first, second));
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

            Assert.IsFalse(_coordinateVerifier.CoordinatesAreInProximity(first, second));
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

            Assert.IsFalse(_coordinateVerifier.CoordinatesAreInProximity(first, second));
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

        private static Coordinate[] GetSquarePolygon()
        {
            return new []
            {
                new Coordinate(1, 1),
                new Coordinate(2, 1), //   ____
                new Coordinate(2, 2), //  |    |
                new Coordinate(1, 2), //  |____|
            };
        }

        private static Coordinate[] GetZiggyPolygon()
        {
//            1             2         3
//
//     2      X------------------------X
//            XX                     XXX
//             XX                 XXX
//     3        XX               XX
//               XX            XX
//                XX          XX
//                 XX       XXX
//     4            XX      XX
//                  X        XX
//                 XX         XX
//     5          XX           XXX
//               XX              XX
//              X                 XX
//             XX                  XX
//     6      XXXXXXXXXXXXXXXXXXXXXXX

            return new []
            {
                new Coordinate(1,2), 
                new Coordinate(2,2), 
                new Coordinate(3,2), 
                new Coordinate(2,4), 
                new Coordinate(3,6), 
                new Coordinate(1,6), 
                new Coordinate(0.5, 4), 
            };
        }
    }
}
