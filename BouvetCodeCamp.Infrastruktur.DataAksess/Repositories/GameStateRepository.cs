using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.Infrastruktur.DataAksess.Interfaces;
using Microsoft.Azure.Documents;

namespace BouvetCodeCamp.Infrastruktur.DataAksess.Repositories
{
    using log4net;

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
