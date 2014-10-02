using System.Collections.Generic;
using System.Threading.Tasks;
using BouvetCodeCamp.Domene.Entiteter;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    using System;

    public interface ILagService
    {
        Lag HentLagMedLagId(string lagId);

        IEnumerable<Lag> HentAlleLag();

        Task Oppdater(Lag lag);

        Lag Hent(string id);

        Task SlettAlle();

        Task Slett(Lag lag);

        IEnumerable<Lag> Søk(Func<Lag, bool> predicate);

        Task Opprett(Lag lag);
    }
}
