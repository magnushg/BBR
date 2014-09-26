using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    using BouvetCodeCamp.Domene.Entiteter;

    public interface IRepository<T>
    {
        String CollectionId { get; }
        
        Task<string> Opprett(T document);

        IEnumerable<T> HentAlle();

        T Hent(string id);

        Task Oppdater(T document);

        Task Slett(T document);

        Task SlettAlle();

        T Søk(Func<T, bool> predicate);
    }
}