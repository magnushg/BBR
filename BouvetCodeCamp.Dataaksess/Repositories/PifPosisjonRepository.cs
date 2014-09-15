using System.Linq;
using BouvetCodeCamp.Felles.Konfigurasjon;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;

namespace BouvetCodeCamp.Dataaksess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Interfaces;
    using Felles;
    using Felles.Entiteter;

    public class PifPosisjonRepository : IPifPosisjonRepository
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
                    _collectionId = _konfigurasjon.HentAppSetting(DocumentDbKonstanter.PifPosisjonerCollectionId);
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
        
        public PifPosisjonRepository(IKonfigurasjon konfigurasjon, IDocumentDbContext context)
        {
            _konfigurasjon = konfigurasjon;
            Context = context;
        }

        public async Task<Document> Opprett(PifPosisjon document)
        {
            return await Context.Client.CreateDocumentAsync(Collection.SelfLink, document);
        }

        public async Task<IEnumerable<PifPosisjon>> HentAlle()
        {
            return await Task.Run(() =>
                Context.Client.CreateDocumentQuery<PifPosisjon>(Collection.DocumentsLink)
                    .AsEnumerable()
                    .ToList());
        }

        public async Task<IEnumerable<PifPosisjon>> HentPifPosisjonerForLag(string lagId)
        {
            return await Task.Run(() =>
                Context.Client.CreateDocumentQuery<PifPosisjon>(Collection.DocumentsLink)
                    .Where(o => o.LagId == lagId)
                    .AsEnumerable()
                    .ToList()
                    .OrderByDescending(o => o.Tid));
        }
        
        public async Task<PifPosisjon> Hent(string id)
        {
            return await Task.Run(() =>
                Context.Client.CreateDocumentQuery<PifPosisjon>(Collection.DocumentsLink)
                .Where(d => d.Id == id)
                .AsEnumerable()
                .FirstOrDefault());
        }

        public async Task Oppdater(PifPosisjon document)
        {
            var pifPosisjon = Context.Client.CreateDocumentQuery<Document>(Collection.DocumentsLink)
                        .Where(d => d.Id == document.Id)
                        .AsEnumerable().FirstOrDefault();

            if (pifPosisjon == null)
                throw new Exception("Fant ikke pifposisjonen som skulle oppdateres.");

            await Context.Client.ReplaceDocumentAsync(pifPosisjon.SelfLink, document);
        }
    }
}