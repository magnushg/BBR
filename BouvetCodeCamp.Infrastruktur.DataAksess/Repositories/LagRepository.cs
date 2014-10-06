namespace BouvetCodeCamp.Infrastruktur.DataAksess.Repositories
{
    using System;

    using Domene.Entiteter;
    using Interfaces;

    public class LagRepository : Repository<Lag>
    {
        private string _collectionId;

        public LagRepository(IKonfigurasjon konfigurasjon, IDocumentDbContext context) : base(konfigurasjon, context)
        {
        }

        public override String CollectionId
        {
            get
            {
                if (string.IsNullOrEmpty(_collectionId))
                {
                    _collectionId = _konfigurasjon.HentAppSetting(DocumentDbKonstanter.LagCollectionId);
                }

                return _collectionId;
            }
        }

    }
}