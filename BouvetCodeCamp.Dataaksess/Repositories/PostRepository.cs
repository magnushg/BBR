namespace BouvetCodeCamp.Infrastruktur.DataAksess.Repositories
{
    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.Infrastruktur.DataAksess.Interfaces;

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
                if (string.IsNullOrEmpty(this._collectionId))
                {
                    this._collectionId = this._konfigurasjon.HentAppSetting(DocumentDbKonstanter.PostCollectionId);
                }

                return this._collectionId;
            }
        }
    }
}
