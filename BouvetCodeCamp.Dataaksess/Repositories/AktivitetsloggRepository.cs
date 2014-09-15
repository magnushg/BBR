using BouvetCodeCamp.Felles.Konfigurasjon;
using Microsoft.Azure.Documents;

namespace BouvetCodeCamp.Dataaksess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Interfaces;
    using Felles;
    using Felles.Entiteter;
    using Microsoft.Azure.Documents.Linq;

    public class AktivitetsloggRepository : IAktivitetsloggRepository
    {
        private readonly IKonfigurasjon _konfigurasjon;
        private readonly IDocumentDbContext Context;

        private string _collectionId;
        public String CollectionId
        {
            get
            {
                if (string.IsNullOrEmpty(_collectionId))
                {
                    _collectionId = _konfigurasjon.HentAppSetting(DocumentDbKonstanter.AktivitetsloggEntriesCollectionId);
                }

                return _collectionId;
            }
        }

        private DocumentCollection _collection;
        public DocumentCollection Collection
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
        
        public AktivitetsloggRepository(IKonfigurasjon konfigurasjon, IDocumentDbContext context)
        {
            _konfigurasjon = konfigurasjon;
            Context = context;
        }

        public async Task Opprett(AktivitetsloggEntry document)
        {
            await Context.Client.CreateDocumentAsync(Collection.SelfLink, document);
        }

        public async Task<IEnumerable<AktivitetsloggEntry>> HentAlle()
        {
            return await Task.Run(() =>
                Context.Client.CreateDocumentQuery<AktivitetsloggEntry>(Collection.DocumentsLink)
                    .AsEnumerable()
                    .ToList());
        }
    }
}