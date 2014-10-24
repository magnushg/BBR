namespace Bouvet.BouvetBattleRoyale.Unittests.SpillOppretter
{
    using System;
    using System.Configuration;

    using BouvetCodeCamp.SpillOppretter;

    using NUnit.Framework;

    using Should;

    [TestFixture]
    class LagOppretterTests
    {
        private LagOppretter lagOppretter;

        [SetUp]
        public void Setup()
        {
            var filePath = string.Format("importData/{0}", ConfigurationManager.AppSettings["location"]);

            lagOppretter = new LagOppretter(Convert.ToInt32(ConfigurationManager.AppSettings["numberOfTeams"]), filePath + "/lagPoster.json", filePath + "/koder.json");  
        }

        [Test]
        public void ShaChecksum() //TODO: lag test for checksumgenerering
        {
            // Arrange

            // Act
            var result = lagOppretter.ShaChecksum("Lag 1", 0);
            var anotherId = lagOppretter.CreateGuid();
            var yetAnotherId = lagOppretter.Sha256("Lag 1" + "0");

            // Assert
            result.ShouldNotBeEmpty();
        }
    }
}