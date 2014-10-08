namespace BouvetCodeCamp.DomeneTjenester.Services
{
    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;

    public class LagService : Service<Lag>
    {
        public LagService(IRepository<Lag> lagRepository) : base(lagRepository)
        {
        }
    }
}