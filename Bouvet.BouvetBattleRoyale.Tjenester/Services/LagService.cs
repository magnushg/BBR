namespace Bouvet.BouvetBattleRoyale.Tjenester.Services
{
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Tjenester.Interfaces;

    public class LagService : Service<Lag>
    {
        public LagService(IRepository<Lag> lagRepository) : base(lagRepository)
        {
        }
    }
}