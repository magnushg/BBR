namespace BouvetCodeCamp.DomeneTjenester.Services
{
    using System;
    using System.Linq;

    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;

    public class LagGameService : ILagGameService
    {
        private readonly IRepository<Lag> _lagRepository;

        public LagGameService(IRepository<Lag> lagRepository)
        {
            _lagRepository = lagRepository;
        }

        public Lag HentLagMedLagId(string lagId)
        {
            try
            {
                var lag = _lagRepository.Søk(o => o.LagId == lagId).First();

                return lag;
            }
            catch (Exception e)
            {
                throw new Exception("Fant ikke lag med lagId: " + lagId, e);
            }
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