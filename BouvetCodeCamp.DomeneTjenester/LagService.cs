using System.Collections.Generic;

using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester.Interfaces;

namespace BouvetCodeCamp.DomeneTjenester
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class LagService : ILagService
    {
        private readonly IRepository<Lag> _lagRepository;

        public LagService(IRepository<Lag> lagRepository)
        {
            _lagRepository = lagRepository;
        }

        public Lag HentLagMedLagId(string lagId)
        {
            var lag = _lagRepository.Søk(o => o.LagId == lagId).First();

            if (lag == null)
                throw new Exception("Fant ikke lag med lagId: " + lagId);

            return lag;
        }

        public IEnumerable<Lag> HentAlleLag()
        {
            return _lagRepository.HentAlle();
        }

        public async Task Oppdater(Lag lag)
        {
            await _lagRepository.Oppdater(lag);
        }

        public Lag Hent(string id)
        {
            return _lagRepository.Hent(id);
        }

        public async Task SlettAlle()
        {
            await _lagRepository.SlettAlle();
        }

        public async Task Slett(Lag lag)
        {
            await _lagRepository.Slett(lag);
        }

        public IEnumerable<Lag> Søk(Func<Lag, bool> predicate)
        {
            return _lagRepository.Søk(predicate);
        }

        public Task Opprett(Lag lag)
        {
            return _lagRepository.Opprett(lag);
        }

        public PifPosisjon HentSistePifPosisjon(string lagId)
        {
            var lag = HentLagMedLagId(lagId);
            var sortertListe = lag.PifPosisjoner.OrderByDescending(x => x.Tid);
            var nyeste = sortertListe.FirstOrDefault();

            return nyeste;
        }
    }
}