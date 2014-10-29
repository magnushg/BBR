namespace Bouvet.BouvetBattleRoyale.Tjenester.Services
{
    using Bouvet.BouvetBattleRoyale.Tjenester.Interfaces;

    using BouvetCodeCamp.Domene.Entiteter;

    public class LagService : Service<Lag>
    {
        public LagService(IRepository<Lag> lagRepository) : base(lagRepository)
        {
        }
    }
}