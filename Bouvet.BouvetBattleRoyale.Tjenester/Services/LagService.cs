namespace BouvetCodeCamp.DomeneTjenester.Services
{
    using Domene.Entiteter;
    using Interfaces;

    public class LagService : Service<Lag>
    {
        public LagService(IRepository<Lag> lagRepository) : base(lagRepository)
        {
        }
    }
}