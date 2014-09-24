namespace BouvetCodeCamp.Infrastruktur.DataAksess.Repositories
{
    using System;

    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.Infrastruktur.DataAksess.Interfaces;

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
                if (string.IsNullOrEmpty(this._collectionId))
                {
                    this._collectionId = this._konfigurasjon.HentAppSetting(DocumentDbKonstanter.LagCollectionId);
                }

                return this._collectionId;
            }
        }

    }
}