using System;
using Microsoft.Azure.Documents;

namespace BouvetCodeCamp.Dataaksess.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepository<T>
    {
        String CollectionId { get; }
        
        DocumentCollection Collection { get; }
        
        Task<Document> Opprett(T document);

        Task<IEnumerable<T>> HentAlle();

        Task<T> Hent(string id);

        Task Oppdater(T document);
    }
}