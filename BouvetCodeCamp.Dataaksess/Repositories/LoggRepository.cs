using BouvetCodeCamp.Dataaksess.Interfaces;
using BouvetCodeCamp.Domene.Entiteter;

using Microsoft.Azure.Documents;

namespace BouvetCodeCamp.Dataaksess.Repositories
{
    public class LoggRepository : Repository<LoggHendelse>
    {
        private string _collectionId;
        private DocumentCollection _collection;

        public LoggRepository(IKonfigurasjon konfigurasjon, IDocumentDbContext context) : base(konfigurasjon, context)
        {
        }

        public override string CollectionId
        {
            get
            {
                if (string.IsNullOrEmpty(_collectionId))
                {
                    _collectionId = _konfigurasjon.HentAppSetting(DocumentDbKonstanter.LoggCollectionId);
                }

                return _collectionId;
            }
        }
    }
}