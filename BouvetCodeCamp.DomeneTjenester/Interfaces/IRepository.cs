using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    public interface IRepository<T>
    {
        String CollectionId { get; }
        
        Task<string> Opprett(T document);

        Task<IEnumerable<T>> HentAlle();

        Task<T> Hent(string id);

        Task Oppdater(T document);

        Task Slett(T document);
    }
}