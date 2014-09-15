using BouvetCodeCamp.Felles.Konfigurasjon;
using Microsoft.Azure.Documents;

namespace BouvetCodeCamp.Dataaksess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;

    using Interfaces;
    using Felles;
    using Felles.Entiteter;
    using Microsoft.Azure.Documents.Linq;

    using Newtonsoft.Json;

    public class MeldingRepository : BaseRepository, IMeldingRepository
    {
        private string _collectionId;
        public String CollectionId
        {
            get
            {
                if (string.IsNullOrEmpty(_collectionId))
                {
                    _collectionId = ConfigurationManager.AppSettings[DocumentDbKonstanter.MeldingerCollectionId];
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
                    ReadOrCreateCollection(Database.SelfLink).Wait();
                }

                return _collection;
            }
        }

        protected override async Task ReadOrCreateCollection(string databaseLink)
        {
            var collections = Client.CreateDocumentCollectionQuery(databaseLink)
                              .Where(col => col.Id == CollectionId).ToArray();

            if (collections.Any())
            {
                _collection = collections.First();
            }
            else
            {
                _collection = await Client.CreateDocumentCollectionAsync(databaseLink,
                    new DocumentCollection { Id = CollectionId });
            }
        }

        public MeldingRepository(IKonfigurasjon konfigurasjon)
            : base(konfigurasjon)
        {
        }

        public async Task Opprett(Melding document)
        {
            await Client.CreateDocumentAsync(Collection.SelfLink, document);
        }

        public async Task<IEnumerable<Melding>> HentAlle()
        {
            return await Task.Run(() =>
                Client.CreateDocumentQuery<Melding>(Collection.DocumentsLink)
                    .AsEnumerable()
                    .ToList());
        }
    }
}