using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace BouvetCodeCamp.Dataaksess.Interfaces
{
    using BouvetCodeCamp.Felles.Entiteter;

    public interface IAktivitetsloggRepository : IRepository<AktivitetsloggHendelse>
    {
        
    }

    public class FakeAktivitetsloggRepo : IAktivitetsloggRepository
    {
        public string CollectionId { get; private set; }
        public DocumentCollection Collection { get; private set; }
        public Task<Document> Opprett(AktivitetsloggHendelse document)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<AktivitetsloggHendelse>> HentAlle()
        {
            throw new System.NotImplementedException();
        }

        public Task<AktivitetsloggHendelse> Hent(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task Oppdater(AktivitetsloggHendelse document)
        {
            throw new System.NotImplementedException();
        }
    }
}