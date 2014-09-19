using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BouvetCodeCamp.Dataaksess.Interfaces;
using BouvetCodeCamp.Felles;
using BouvetCodeCamp.Felles.Entiteter;
using BouvetCodeCamp.Felles.Konfigurasjon;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;

namespace BouvetCodeCamp.Dataaksess.Repositories
{
    public class LagRepository : ILagRepository
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
                    _collectionId = _konfigurasjon.HentAppSetting(DocumentDbKonstanter.LagCollectionId);
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

        public LagRepository(IKonfigurasjon konfigurasjon, IDocumentDbContext context)
        {
            _konfigurasjon = konfigurasjon;
            Context = context;
        }

        public async Task<Document> Opprett(Lag document)
        {
            return await Context.Client.CreateDocumentAsync(Collection.SelfLink, document);
        }
        
        public async Task<IEnumerable<Lag>> HentAlle()
        {
            return await Task.Run(() =>
                Context.Client.CreateDocumentQuery<Lag>(Collection.DocumentsLink)
                    .AsEnumerable()
                    .ToList());
        }

        public async Task<Lag> Hent(string id)
        {
            return await Task.Run(() =>
                Context.Client.CreateDocumentQuery<Lag>(Collection.DocumentsLink)
                .Where(d => d.Id == id)
                .AsEnumerable()
                .FirstOrDefault());
        }

        public async Task Oppdater(Lag document)
        {
            var lag = Context.Client.CreateDocumentQuery<Document>(Collection.DocumentsLink)
                        .Where(d => d.Id == document.Id)
                        .AsEnumerable().FirstOrDefault();

            if (lag == null)
                throw new Exception("Fant ikke laget som skulle oppdateres.");

            await Context.Client.ReplaceDocumentAsync(lag.SelfLink, document);
        }
    }
}