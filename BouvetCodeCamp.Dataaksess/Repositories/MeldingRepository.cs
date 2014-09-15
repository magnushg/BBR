﻿using System.Linq;
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

    public class MeldingRepository : IMeldingRepository
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
                    _collectionId = _konfigurasjon.HentAppSetting(DocumentDbKonstanter.MeldingerCollectionId);
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

        public MeldingRepository(IKonfigurasjon konfigurasjon, IDocumentDbContext context)
        {
            _konfigurasjon = konfigurasjon;
            Context = context;
        }

        public async Task<Document> Opprett(Melding document)
        {
            return await Context.Client.CreateDocumentAsync(Collection.SelfLink, document);
        }

        public async Task<IEnumerable<Melding>> HentAlle()
        {
            return await Task.Run(() =>
                Context.Client.CreateDocumentQuery<Melding>(Collection.DocumentsLink)
                    .AsEnumerable()
                    .ToList());
        }
        
        public async Task<Melding> Hent(string id)
        {
            return await Task.Run(() =>
                Context.Client.CreateDocumentQuery<Melding>(Collection.DocumentsLink)
                .Where(d => d.Id == id)
                .AsEnumerable()
                .FirstOrDefault());
        }

        public async Task Oppdater(Melding document)
        {
            var melding = Context.Client.CreateDocumentQuery<Document>(Collection.DocumentsLink)
                        .Where(d => d.Id == document.Id)
                        .AsEnumerable().FirstOrDefault();

            if (melding == null)
                throw new Exception("Fant ikke meldingen som skulle oppdateres.");

            await Context.Client.ReplaceDocumentAsync(melding.SelfLink, document);
        }
    }
}