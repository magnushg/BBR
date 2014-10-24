namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Data.Repositories
{
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Data.Interfaces;

    using log4net;

    using Microsoft.Azure.Documents;

    public class GameStateRepository : Repository<GameState>
    {
        private string _collectionId;
        private DocumentCollection _collection;

        public GameStateRepository(
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
                    _collectionId = _konfigurasjon.HentAppSetting(DocumentDbKonstanter.GameStateCollectionId);
                }

                return _collectionId;
            }
        }
    }
}
