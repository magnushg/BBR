namespace Bouvet.BouvetBattleRoyale.Integrasjonstester.Infrastruktur.CrossCutting
{
    using System;
    using System.Configuration;

    using Bouvet.BouvetBattleRoyale.Infrastruktur.CrossCutting;

    using NUnit.Framework;

    using Should;

    [TestFixture]
    public class KonfigurasjonTests
    {
        private Konfigurasjon _konfigurasjon;

        [SetUp]
        public void SetUp()
        {
            _konfigurasjon = new Konfigurasjon();
        }
        
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void HentAppsetting_KeyErNull_KasterException()
        {
            // Arrange
            string nullKey = null;

            // Act
            _konfigurasjon.HentAppSetting(nullKey);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void HentAppsetting_KeyErTomString_KasterException()
        {
            // Arrange
            string tomKey = string.Empty;

            // Act
            _konfigurasjon.HentAppSetting(tomKey);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void HentAppsetting_UkjentAppSetting_KasterException()
        {
            // Arrange
            const string UkjentKey = "ukjentsettingkey";

            // Act
            _konfigurasjon.HentAppSetting(UkjentKey);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void HentAppsetting_TomAppSetting_KasterException()
        {
            // Arrange
            const string UkjentKey = "TomDummySetting";
            
            // Act
            _konfigurasjon.HentAppSetting(UkjentKey);
        }

        [Test]
        public void HentAppsetting_AppSettingEksisterer_GirSettingVerdi()
        {
            // Arrange
            const string Key = "DocumentDbDatabaseNavn";

            // Act
            var resultat = _konfigurasjon.HentAppSetting(Key);

            // Assert
            resultat.ShouldEqual("BouvetCodeCamp2014Integrasjonstest");
        }
    }
}