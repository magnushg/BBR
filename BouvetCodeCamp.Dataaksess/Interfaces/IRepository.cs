using System;
using Microsoft.Azure.Documents;

namespace BouvetCodeCamp.Dataaksess.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepository<T>
    {
        Task Opprett(T document);

        Task<IEnumerable<T>> HentAlle();

        String CollectionId { get; }

        DocumentCollection Collection { get; }
    }
}