using System.Threading.Tasks;
using BouvetCodeCamp.Dataaksess.Interfaces;
using BouvetCodeCamp.Felles.Entiteter;
using BouvetCodeCamp.Service.Interfaces;

namespace BouvetCodeCamp.Service.Services
{
    public class LagService : ILagService
    {
        private readonly ILagRepository _lagRepository;

        public LagService(ILagRepository lagRepository)
        {
            _lagRepository = lagRepository;
        }

        public Task<Lag> HentLag(string lagId)
        {
            return _lagRepository.Hent(lagId);
        }

        public void Oppdater(Lag lag)
        {
            _lagRepository.Oppdater(lag);
        }
    }
}
