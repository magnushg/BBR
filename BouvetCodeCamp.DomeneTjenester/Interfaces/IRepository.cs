using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    public interface IRepository<T>
    {
        String CollectionId { get; }
        
        DocumentCollection Collection { get; }
        
        Task<Document> Opprett(T document);

        Task<IEnumerable<T>> HentAlle();

        Task<T> Hent(string id);

        Task Oppdater(T document);

        Task Slett(T document);
    }
}