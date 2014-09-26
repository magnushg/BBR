using System.Collections.Generic;

using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester.Interfaces;

namespace BouvetCodeCamp.DomeneTjenester
{
    public class LagService : ILagService
    {
        private readonly IRepository<Lag> _lagRepository;

        public LagService(IRepository<Lag> lagRepository)
        {
            _lagRepository = lagRepository;
        }

        public Lag HentLag(string lagId)
        {
            return _lagRepository.Søk(o => o.LagId == lagId);
        }

        public IEnumerable<Lag> HentAlleLag()
        {
            return _lagRepository.HentAlle();
        }

        public void Oppdater(Lag lag)
        {
            _lagRepository.Oppdater(lag);
        }
    }
}