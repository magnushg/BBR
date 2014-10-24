namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Data.Repositories
{
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Data.Interfaces;

    using BouvetCodeCamp.Domene.Entiteter;

    using log4net;

    using Microsoft.Azure.Documents;

    public class PostRepository : Repository<Post>
    {
        private string _collectionId;
        private DocumentCollection _collection;

        public PostRepository(
            IKonfigurasjon konfigurasjon, 
            IDocumentDbContext context, 
            ILog log) : base(konfigurasjon, context, log)
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
