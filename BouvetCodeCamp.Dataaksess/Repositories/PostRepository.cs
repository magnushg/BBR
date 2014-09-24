using BouvetCodeCamp.Dataaksess.Interfaces;
using BouvetCodeCamp.Domene.Entiteter;

using Microsoft.Azure.Documents;

namespace BouvetCodeCamp.Dataaksess.Repositories
{
    public class PostRepository : Repository<Post>
    {
        private string _collectionId;
        private DocumentCollection _collection;

        public PostRepository(IKonfigurasjon konfigurasjon, IDocumentDbContext context) : base(konfigurasjon, context)
        {
        }

        public override string CollectionId
        {
            get
            {
                if (string.IsNullOrEmpty(_collectionId))
                {
                    _collectionId = _konfigurasjon.HentAppSetting(DocumentDbKonstanter.PostCollectionId);
                }

                return _collectionId;
            }
        }
    }
}
