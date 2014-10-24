using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester;
using BouvetCodeCamp.DomeneTjenester.Interfaces;
using NUnit.Framework;

namespace BouvetCodeCamp.UnitTest.Felles
{
    using Should;

    [TestFixture]
    public class KoordinatTests
    {
        private IKoordinatVerifier koordinatVerifier;

        [SetUp]
        public void Setup()
        {
            this.koordinatVerifier = new KoordinatVerifier();
        }

        [Test]
        public void KoordinaterErNærHverandre_EkteKoordinater_Nær1()
        {
            //59.676242, 10.604637
            var koordinat1 = new Koordinat("59.676242, 10.604637");
            var koordinat2 = new Koordinat("59.675767, 10.604851");

            Assert.IsTrue(koordinatVerifier.KoordinaterErNærHverandre(koordinat1, koordinat2));
        }

        [Test]
        public void KoordinaterErNærHverandre_EkteKoordinater_RadiusOver100()
        {
            //59.676242, 10.604637
            var koordinat1 = new Koordinat("59.675372, 10.606590");
            var koordinat2 = new Koordinat("59.674711, 10.605179");

            Assert.IsFalse(koordinatVerifier.KoordinaterErNærHverandre(koordinat1, koordinat2));
        }

        [Test]
        public void KoordinaterErNærHverandre_EkteKoordinater_LangtLongitude()
        {
            //59.676242, 10.604637
            var koordinat1 = new Koordinat("59.676325, 10.604862");
            var koordinat2 = new Koordinat("59.675632, 10.599541");

            Assert.IsFalse(koordinatVerifier.KoordinaterErNærHverandre(koordinat1, koordinat2));
        }

        [Test]
        public void KoordinaterErNærHverandre_EkteKoordinater_SemiLatitude()
        {
            //59.676242, 10.604637
            var koordinat1 = new Koordinat("59.675849, 10.606193");
            var koordinat2 = new Koordinat("59.676878, 10.606579");

            Assert.IsFalse(koordinatVerifier.KoordinaterErNærHverandre(koordinat1, koordinat2));
        }

        [Test]
        public void KoordinaterErNærHverandre_EkteKoordinater_SemiLongitude()
        {
            //59.676242, 10.604637
            var koordinat1 = new Koordinat("59.675849, 10.606193");
            var koordinat2 = new Koordinat("59.675800, 10.604289");

            Assert.IsFalse(koordinatVerifier.KoordinaterErNærHverandre(koordinat1, koordinat2));
        }

        ///
        /// KoordinatErInnenforPolygonet
        ///
        [Test]
        public void KoordinatErInnenforPolygonet_InsidePolygon_True()
        {
            Koordinat[] polygon = GetSquarePolygon();
            var point = new Koordinat(1.5, 1.5);

            Assert.IsTrue(this.koordinatVerifier.KoordinatErInnenforPolygonet(point, polygon));
        }

        [Test]
        public void KoordinatErInnenforPolygonet_LeftOfPolygon_False()
        {
            Koordinat[] polygon = GetSquarePolygon();
            var point = new Koordinat(0.5, 1);

            Assert.IsFalse(this.koordinatVerifier.KoordinatErInnenforPolygonet(point, polygon));
        }

        [Test]
        public void KoordinatErInnenforPolygonet_RightOfPolygon_False()
        {
            Koordinat[] polygon = GetSquarePolygon();
            var point = new Koordinat(2.5, 1.5);

            Assert.IsFalse(this.koordinatVerifier.KoordinatErInnenforPolygonet(point, polygon));
        }

        [Test]
        public void KoordinatErInnenforPolygonet_AbovePolygon_False()
        {
            Koordinat[] polygon = GetSquarePolygon();
            var point = new Koordinat(0.5, 1.5);

            Assert.IsFalse(this.koordinatVerifier.KoordinatErInnenforPolygonet(point, polygon));
        }

        [Test]
        public void KoordinatErInnenforPolygonet_UnderPolygon_False()
        {
            Koordinat[] polygon = GetSquarePolygon();
            var point = new Koordinat(2.5, 1);

            Assert.IsFalse(this.koordinatVerifier.KoordinatErInnenforPolygonet(point, polygon));
        }

        [Test]
        public void KoordinatErInnenforPolygonet_BottomRightCorner_True()
        {
            Koordinat[] polygon = GetSquarePolygon();
            var point = new Koordinat(2, 2);

            Assert.IsTrue(this.koordinatVerifier.KoordinatErInnenforPolygonet(point, polygon));
        }

        [Test]
        public void KoordinatErInnenforPolygonet_BottomLeftCorner_False()
        {
            Koordinat[] polygon = GetSquarePolygon();
            var point = new Koordinat(1, 2);

            Assert.IsFalse(this.koordinatVerifier.KoordinatErInnenforPolygonet(point, polygon));
        }

        [Test]
        public void KoordinatErInnenforPolygonet_TopLeftCorner_False()
        {
            Koordinat[] polygon = GetSquarePolygon();
            var point = new Koordinat(1, 1);

            Assert.IsFalse(this.koordinatVerifier.KoordinatErInnenforPolygonet(point, polygon));
        }

        [Test]
        public void KoordinatErInnenforPolygonet_TopRightCorner_False()
        {
            Koordinat[] polygon = GetSquarePolygon();
            var point = new Koordinat(1, 2);

            Assert.IsFalse(this.koordinatVerifier.KoordinatErInnenforPolygonet(point, polygon));
        }

        [Test]
        public void KoordinatErInnenforPolygonet_GetZiggyPolygon_Case1()
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
            Koordinat[] polygon = GetZiggyPolygon();
            var point = new Koordinat(4, 2.5);

            Assert.IsFalse(this.koordinatVerifier.KoordinatErInnenforPolygonet(point, polygon));
        }

        [Test]
        public void KoordinatErInnenforPolygonet_GetZiggyPolygon_Case2()
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
            Koordinat[] polygon = GetZiggyPolygon();
            var point = new Koordinat(2, 3);

            Assert.IsTrue(this.koordinatVerifier.KoordinatErInnenforPolygonet(point, polygon));
        }

        [Test]
        public void KoordinatErInnenforPolygonet_GetZiggyPolygon_Case3()
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
            Koordinat[] polygon = GetZiggyPolygon();
            var point = new Koordinat(2, 2);

            Assert.IsFalse(this.koordinatVerifier.KoordinatErInnenforPolygonet(point, polygon));
        }

        [Test]
        public void KoordinatErInnenforPolygonet_GetZiggyPolygon_Case4()
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
            Koordinat[] polygon = GetZiggyPolygon();
            var point = new Koordinat(1.49, 4);

            Assert.IsTrue(this.koordinatVerifier.KoordinatErInnenforPolygonet(point, polygon));
        }

        [Test]
        public void KoordinatErInnenforPolygonet_GetZiggyPolygon_Case5()
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
            Koordinat[] polygon = GetZiggyPolygon();
            var point = new Koordinat(1.7, 7);

            Assert.IsFalse(this.koordinatVerifier.KoordinatErInnenforPolygonet(point, polygon));
        }

        ///
        /// KoordinaterErNærHverandre
        ///
        [Test]
        public void KoordinaterErNærHverandre_PerfectMatch_ReturnsTrue()
        {
            Koordinat first = new Koordinat("12", "12"),
                second = new Koordinat("12", "12");

            Assert.IsTrue(this.koordinatVerifier.KoordinaterErNærHverandre(first, second));
        }

        ///
        /// KoordinaterErNærHverandre
        ///
        [Test]
        public void KoordinaterErNærHverandre_PerfectMatchWithNoDecimals_ReturnsTrue()
        {
            Koordinat first = new Koordinat("12", "12"),
                second = new Koordinat("12", "12");

            Assert.IsTrue(this.koordinatVerifier.KoordinaterErNærHverandre(first, second));
        }

        [Test]
        public void KoordinaterErNærHverandre_LatOutsideThreshold_ReturnsFalse()
        {
            Koordinat first = new Koordinat("10", "10");
            var latThreshold = KoordinatVerifier.LatProximityThreshold;
            var longThreshold = KoordinatVerifier.LongProximityThreshold;

            var lngd = (10 + longThreshold);
            var latd = (10 + latThreshold) + 1;

            var second = new Koordinat(lngd.ToString(), latd.ToString());

            Assert.IsFalse(this.koordinatVerifier.KoordinaterErNærHverandre(first, second));
        }

        [Test]
        public void KoordinaterErNærHverandre_LongOutsideThreshold_ReturnsFalse()
        {
            Koordinat first = new Koordinat("10", "10");
            var latThreshold = KoordinatVerifier.LatProximityThreshold;
            var longThreshold = KoordinatVerifier.LongProximityThreshold;

            var lngd = (10 + longThreshold) + 1;
            var latd = (10 + latThreshold);

            var second = new Koordinat(lngd.ToString(), latd.ToString());

            Assert.IsFalse(this.koordinatVerifier.KoordinaterErNærHverandre(first, second));
        }

        [Test]
        public void ErStringEtGyldigKoordinat_ValidPositiveNoWhitespaceCoord_ReturnsTrue()
        {
            string c ="40,40";

            Assert.IsTrue(Koordinat.ErStringEtGyldigKoordinat(c));
        }

        [Test]
        public void ErStringEtGyldigKoordinat_ValidPositiveWithWhitespaceCoord_ReturnsTrue()
        {
            string c ="40, 40";

            Assert.IsTrue(Koordinat.ErStringEtGyldigKoordinat(c));
        }

        [Test]
        public void ErStringEtGyldigKoordinat_ValidSecondNegativeWithWhitespaceCoord_ReturnsTrue()
        {
            string c = "40, -40";

            Assert.IsTrue(Koordinat.ErStringEtGyldigKoordinat(c));
        }

        [Test]
        public void ErStringEtGyldigKoordinat_ValidFirstNegativeWithWhitespaceCoord_ReturnsTrue()
        {
            string c = "-40, 40";

            Assert.IsTrue(Koordinat.ErStringEtGyldigKoordinat(c));
        }

        [Test]
        public void ErStringEtGyldigKoordinat_ValidSecondNegativeNoWhitespaceCoord_ReturnsTrue()
        {
            string c = "40,-40";

            Assert.IsTrue(Koordinat.ErStringEtGyldigKoordinat(c));
        }

        [Test]
        public void ErStringEtGyldigKoordinat_ValidFirstNegativeWhitespaceCoord_ReturnsTrue()
        {
            string c = "-40,40";

            Assert.IsTrue(Koordinat.ErStringEtGyldigKoordinat(c));
        }

        [Test]
        //ble lei av å skrive unit test for hvert case
        public void ErStringEtGyldigKoordinat_BadMatches_ReturnsFalse()
        {
            string c1 = "-90., -180",
                c2 = "+90.1, -100.111",
                c3 = "-91, 123.456",
                c4 = "045, 180",
                c5 = "90,",
                c6 = ",45";

            Assert.IsFalse(Koordinat.ErStringEtGyldigKoordinat(c1));
            Assert.IsFalse(Koordinat.ErStringEtGyldigKoordinat(c2));
            Assert.IsFalse(Koordinat.ErStringEtGyldigKoordinat(c3));
            Assert.IsFalse(Koordinat.ErStringEtGyldigKoordinat(c4));
        }

        [Test]
        public void ErStringEtGyldigKoordinat_LatitudeOutOfRange_ReturnsFalse()
        {
            string under = "45.0, -180.1", over = "45.0, 180.1";

            Assert.IsFalse(Koordinat.ErStringEtGyldigKoordinat(under));
            Assert.IsFalse(Koordinat.ErStringEtGyldigKoordinat(over));
        }

        [Test]
        public void ErStringEtGyldigKoordinat_LongitudeOfRange_ReturnsFalse()
        {
            string under = "90.1, -180.0", over = "90.1, 180.0";

            Assert.IsFalse(Koordinat.ErStringEtGyldigKoordinat(under));
            Assert.IsFalse(Koordinat.ErStringEtGyldigKoordinat(over));
        }

        [Test]
        public void Koordinat_TomtKoordinat_XErTom()
        {
            var koordinat = new Koordinat("", "");

            koordinat.X.ShouldEqual(0);
            koordinat.Y.ShouldEqual(0);
        }

        private static Koordinat[] GetSquarePolygon()
        {
            return new []
            {
                new Koordinat(1, 1),
                new Koordinat(2, 1), //   ____
                new Koordinat(2, 2), //  |    |
                new Koordinat(1, 2), //  |____|
            };
        }

        private static Koordinat[] GetZiggyPolygon()
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
                new Koordinat(1,2), 
                new Koordinat(2,2), 
                new Koordinat(3,2), 
                new Koordinat(2,4), 
                new Koordinat(3,6), 
                new Koordinat(1,6), 
                new Koordinat(0.5, 4), 
            };
        }
    }
}
