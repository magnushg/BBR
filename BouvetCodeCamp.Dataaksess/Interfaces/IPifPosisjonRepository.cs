using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace BouvetCodeCamp.Dataaksess.Interfaces
{
    using Felles.Entiteter;

    public interface IPifPosisjonRepository : IRepository<PifPosisjon>
    {
        Task<IEnumerable<PifPosisjon>> HentPifPosisjonerForLag(string lagId);
    }

    public class PifRepoFake : IPifPosisjonRepository
    {
        public string CollectionId { get; private set; }
        public DocumentCollection Collection { get; private set; }
        public Task<Document> Opprett(PifPosisjon document)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<PifPosisjon>> HentAlle()
        {
            throw new System.NotImplementedException();
        }

        public Task<PifPosisjon> Hent(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task Oppdater(PifPosisjon document)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<PifPosisjon>> HentPifPosisjonerForLag(string lagId)
        {
            throw new System.NotImplementedException();
        }
    }
}