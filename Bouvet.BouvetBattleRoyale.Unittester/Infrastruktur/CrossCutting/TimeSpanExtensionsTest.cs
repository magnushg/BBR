namespace Bouvet.BouvetBattleRoyale.Unittests.Infrastruktur.CrossCutting
{
    using System;

    using Bouvet.BouvetBattleRoyale.Infrastruktur.CrossCutting.Extensions;

    using NUnit.Framework;

    using Should;

    [TestFixture]
    public class TimeSpanExtensionsTest
    {
        [TestCase(5, "5 milliseconds")]
        [TestCase(1, "1 millisecond")]
        public void ToReadableString_TimeSpanWithMilliseconds_ReturnsReadableString(int milliseconds, string expectedResult)
        {
            // Arrange
            var timespan = TimeSpan.FromMilliseconds(milliseconds);

            // Act
            var resultat = timespan.ToReadableString();

            // Assert
            resultat.ShouldEqual(expectedResult);
        }

        [TestCase(5, "5 seconds")]
        [TestCase(1, "1 second")]
        public void ToReadableString_TimeSpanWithSeconds_ReturnsReadableString(int seconds, string expectedResult)
        {
            // Arrange
            var timespan = TimeSpan.FromSeconds(seconds);

            // Act
            var resultat = timespan.ToReadableString();
            
            // Assert
            resultat.ShouldEqual(expectedResult);
        }

        [TestCase(5, "5 minutes")]
        [TestCase(1, "1 minute")]
        public void ToReadableString_TimeSpanWithMinutes_ReturnsReadableString(int minutes, string expectedResult)
        {
            // Arrange
            var timespan = TimeSpan.FromMinutes(minutes);

            // Act
            var resultat = timespan.ToReadableString();

            // Assert
            resultat.ShouldEqual(expectedResult);
        }

        [TestCase(5, "5 hours")]
        [TestCase(1, "1 hour")]
        public void ToReadableString_TimeSpanWithHours_ReturnsReadableString(int hours, string expectedResult)
        {
            // Arrange
            var timespan = TimeSpan.FromHours(hours);

            // Act
            var resultat = timespan.ToReadableString();

            // Assert
            resultat.ShouldEqual(expectedResult);
        }

        [TestCase(5, 5, "5 minutes, 5 seconds")]
        [TestCase(1, 1, "1 minute, 1 second")]
        public void ToReadableString_TimeSpanWithMinutesAndSeconds_ReturnsReadableString(int minutes, int seconds, string expectedResult)
        {
            // Arrange
            var timespan = new TimeSpan(0, 0, minutes, seconds);

            // Act
            var resultat = timespan.ToReadableString();

            // Assert
            resultat.ShouldEqual(expectedResult);
        }
    }
}