using System.Collections.Generic;

using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester.Interfaces;

namespace BouvetCodeCamp.DomeneTjenester
{
    using System;
    using System.Linq;

    public class LagService : ILagService
    {
        private readonly IRepository<Lag> _lagRepository;

        public LagService(IRepository<Lag> lagRepository)
        {
            _lagRepository = lagRepository;
        }

        public Lag HentLagMedLagId(string lagId)
        {
            var lag = _lagRepository.Søk(o => o.LagId == lagId).FirstOrDefault();

            if (lag == null)
                throw new Exception("Fant ikke lag med lagId: " + lagId);

            return lag;
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