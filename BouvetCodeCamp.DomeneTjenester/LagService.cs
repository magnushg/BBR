using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester.Interfaces;

namespace BouvetCodeCamp.DomeneTjenester
{
    public class LagService : Service<Lag>
    {
        public LagService(IRepository<Lag> lagRepository) : base(lagRepository)
        {
        }
    }
}