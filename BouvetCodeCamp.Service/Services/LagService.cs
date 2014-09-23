using System.Collections.Generic;
using System.Threading.Tasks;

using BouvetCodeCamp.Felles.Entiteter;
using BouvetCodeCamp.Service.Interfaces;

namespace BouvetCodeCamp.Service.Services
{
    using Dataaksess.Interfaces;

    public class LagService : ILagService
    {
        private readonly IRepository<Lag> _lagRepository;

        public LagService(IRepository<Lag> lagRepository)
        {
            _lagRepository = lagRepository;
        }

        public Task<Lag> HentLag(string lagId)
        {
            return _lagRepository.Hent(lagId);
        }

        public Task<IEnumerable<Lag>> HentAlleLag()
        {
            return _lagRepository.HentAlle();
        }

        public void Oppdater(Lag lag)
        {
            _lagRepository.Oppdater(lag);
        }
    }
}