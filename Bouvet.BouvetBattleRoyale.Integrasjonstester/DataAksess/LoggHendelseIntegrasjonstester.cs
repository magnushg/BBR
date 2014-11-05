namespace Bouvet.BouvetBattleRoyale.Integrasjonstester.DataAksess
{
    using System.Threading.Tasks;

    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Interfaces;

    using FizzWare.NBuilder;

    using NUnit.Framework;

    using Should;

    [TestFixture]
    public class LoggHendelseIntegrasjonstester : BaseRepositoryIntegrasjonstest
    {
        [Test]
        [Category(Testkategorier.DataAksess)]
        public async Task Hent_LoggHendelseMedVerdier_LoggHendelseHarVerdier()
        {
            // Arrange
            var repository = Resolve<IRepository<LoggHendelse>>();

            var loggHendelse = Builder<LoggHendelse>.CreateNew()
                .With(o => o.LagId = TestLagId)
                .Build();

            var documentId = await repository.Opprett(loggHendelse);

            // Act
            var opprettetLoggHendelse = repository.Hent(documentId);

            // Assert
            opprettetLoggHendelse.LagId.ShouldEqual(TestLagId);
        }
    }
}