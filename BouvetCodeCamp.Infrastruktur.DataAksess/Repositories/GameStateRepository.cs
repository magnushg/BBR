using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.Infrastruktur.DataAksess.Interfaces;
using Microsoft.Azure.Documents;

namespace BouvetCodeCamp.Infrastruktur.DataAksess.Repositories
{
    public class GameStateRepository : Repository<GameState>
    {
        private string _collectionId;
        private DocumentCollection _collection;

        public GameStateRepository(IKonfigurasjon konfigurasjon, IDocumentDbContext context) : base(konfigurasjon, context)
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
