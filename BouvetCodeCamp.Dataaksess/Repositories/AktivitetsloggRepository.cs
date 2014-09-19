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

        public async Task<Document> Opprett(AktivitetsloggHendelse document)
        {
            return await Context.Client.CreateDocumentAsync(Collection.SelfLink, document);
        }

        public async Task<IEnumerable<AktivitetsloggHendelse>> HentAlle()
        {
            return await Task.Run(() =>
                Context.Client.CreateDocumentQuery<AktivitetsloggHendelse>(Collection.DocumentsLink)
                    .AsEnumerable()
                    .ToList());
        }

        public async Task<AktivitetsloggHendelse> Hent(string id)
        {
            return await Task.Run(() =>
                Context.Client.CreateDocumentQuery<AktivitetsloggHendelse>(Collection.DocumentsLink)
                .Where(d => d.Id == id)
                .AsEnumerable()
                .FirstOrDefault());
        }
        
        public async Task Oppdater(AktivitetsloggHendelse document)
        {
            var aktivitetsloggEntry = Context.Client.CreateDocumentQuery<Document>(Collection.DocumentsLink)
                .Where(d => d.Id == document.Id)
                .AsEnumerable().FirstOrDefault();
            
            if (aktivitetsloggEntry == null)
                throw new Exception("Fant ikke aktivitetsloggentryen som skulle oppdateres.");

            await Context.Client.ReplaceDocumentAsync(aktivitetsloggEntry.SelfLink, document);
        }
    }
}