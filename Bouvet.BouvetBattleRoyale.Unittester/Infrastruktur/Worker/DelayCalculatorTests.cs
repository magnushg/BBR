namespace Bouvet.BouvetBattleRoyale.Unittests.Infrastruktur.Worker
{
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Worker.Brisebois.WindowsAzure;

    using NUnit.Framework;

    using Should;

    [TestFixture]
    public class DelayCalculatorTests
    {
        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(2, 2)]
        [TestCase(4, 8)]
        [TestCase(8, 128)]
        [TestCase(16, 1024)]
        [TestCase(32, 1024)]
        public void ExponentialDelay_NumberOfFailedAttempts_ReturnsExpextedDelayInSeconds(int failedAttempts, int result)
        {
            // Act
            var resultat = DelayCalculator.ExponentialDelay(failedAttempts);

            // Assert
            resultat.ShouldEqual(result);
        }
    }
}