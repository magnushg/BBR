namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Interfaces
{
    using System.Threading.Tasks;

    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;

    public interface IQueueMessageProducer
    {
        Task CreateMessage(LoggHendelse loggHendelse);
    }
}