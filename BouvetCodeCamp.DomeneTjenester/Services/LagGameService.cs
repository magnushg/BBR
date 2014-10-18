namespace BouvetCodeCamp.DomeneTjenester.Services
{
    using System;
    using System.Linq;

    using Domene.Entiteter;
    using Interfaces;

    public class LagGameService : ILagGameService
    {
        private readonly IRepository<Lag> _lagRepository;

        public LagGameService(IRepository<Lag> lagRepository)
        {
            _lagRepository = lagRepository;
        }

        public Lag HentLagMedLagId(string lagId)
        {
            var lag = _lagRepository.Søk(o => o.LagId == lagId);

            if (lag.Count() > 1)
                throw new Exception("Fant flere lag med lagId: " + lagId);

            return lag.First();
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