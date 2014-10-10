using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    public interface IService<T>
    {
        IEnumerable<T> HentAlle();

        Task Oppdater(T entitet);

        T Hent(string id);

        Task SlettAlle();

        Task Slett(T entitet);

        Task Opprett(T entitet);

        IEnumerable<T> Søk(Func<T, bool> predicate);
    }
}