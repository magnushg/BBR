using BouvetCodeCamp.Dataaksess.Interfaces;
using BouvetCodeCamp.Felles;
using BouvetCodeCamp.Felles.Entiteter;
using BouvetCodeCamp.Felles.Konfigurasjon;
using Microsoft.Azure.Documents;

namespace BouvetCodeCamp.Dataaksess.Repositories
{
    public class LoggRepository : Repository<AktivitetsloggHendelse>
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

        public override DocumentCollection Collection
        {
            get
            {
                if (_collection == null)
                {
                    _collection = Context.ReadOrCreateCollection(Context.Database.SelfLink, CollectionId);
                }

                return _collection;
            }
        }
    }
}
