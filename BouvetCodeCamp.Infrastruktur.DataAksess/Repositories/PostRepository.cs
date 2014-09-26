namespace BouvetCodeCamp.Infrastruktur.DataAksess.Repositories
{
    using Domene.Entiteter;
    using Interfaces;

    using Microsoft.Azure.Documents;

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
