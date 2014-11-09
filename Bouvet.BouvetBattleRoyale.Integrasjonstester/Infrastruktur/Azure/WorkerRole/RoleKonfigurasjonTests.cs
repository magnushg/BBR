namespace Bouvet.BouvetBattleRoyale.Integrasjonstester.Infrastruktur.Azure.WorkerRole
{
    using System;

    using Bouvet.BouvetBattleRoyale.Infrastruktur.Azure.WorkerRole;

    using NUnit.Framework;

    [TestFixture]
    public class RoleKonfigurasjonTests
    {
        private RoleKonfigurasjon _roleKonfigurasjon;

        [SetUp]
        public void SetUp()
        {
            _roleKonfigurasjon = new RoleKonfigurasjon();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void HentAppsetting_KeyErNull_KasterException()
        {
            // Arrange
            string nullKey = null;

            // Act
            _roleKonfigurasjon.HentAppSetting(nullKey);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void HentAppsetting_KeyErTomString_KasterException()
        {
            // Arrange
            string tomKey = string.Empty;

            // Act
            _roleKonfigurasjon.HentAppSetting(tomKey);
        }
    }
}