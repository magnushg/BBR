namespace BouvetCodeCamp.Infrastruktur.DataAksess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Domene.Entiteter;
    using DomeneTjenester.Interfaces;
    using Interfaces;

    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;

    public abstract class Repository<T> : IRepository<T> where T : BaseDocument
    {
        public abstract string CollectionId { get; }
        
        protected readonly IKonfigurasjon _konfigurasjon;
        protected readonly IDocumentDbContext Context;

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

        protected Repository(IKonfigurasjon konfigurasjon, IDocumentDbContext context)
        {
            _konfigurasjon = konfigurasjon;
            Context = context;
        }

        public async Task<string> Opprett(T document)
        {
            var opprettetDocument = await Context.Client.CreateDocumentAsync(Collection.SelfLink, document);

            return opprettetDocument.Resource.Id;
        }

        public IEnumerable<T> HentAlle()
        {
            return Context.Client.CreateDocumentQuery<T>(Collection.DocumentsLink)
                .AsEnumerable();
        }

        public T Hent(string id)
        {
            return Context.Client.CreateDocumentQuery<T>(Collection.DocumentsLink)
                .Where(d => d.DocumentId == id)
                .AsEnumerable()
                .FirstOrDefault();
        }

        public async Task Oppdater(T document)
        {
            var entitet = Context.Client.CreateDocumentQuery<Document>(Collection.DocumentsLink)
                .Where(d => d.Id == document.DocumentId)
                .AsEnumerable()
                .FirstOrDefault();

            if (entitet == null)
                throw new Exception("Fant ikke entiteten som skulle oppdateres.");

            await Context.Client.ReplaceDocumentAsync(entitet.SelfLink, document);
        }

        public async Task Slett(T document)
        {
            var entitet = Context.Client.CreateDocumentQuery<Document>(Collection.DocumentsLink)
                .Where(d => d.Id == document.DocumentId)
                .AsEnumerable()
                .FirstOrDefault();

            if (entitet == null)
                throw new Exception("Fant ikke entiteten som skulle slettes.");

            await Context.Client.DeleteDocumentAsync(entitet.SelfLink, new RequestOptions());
        }

        public async Task SlettAlle()
        {
            var entiteter = Context.Client.CreateDocumentQuery<Document>(Collection.DocumentsLink)
                .AsEnumerable();

            if (entiteter != null && entiteter.Any())
            {
                foreach (var document in entiteter)
                {
                    await Context.Client.DeleteDocumentAsync(document.SelfLink, new RequestOptions());
                }
            }
        }

        public T Søk(Func<T, bool> predicate)
        {
            return this.Context.Client.CreateDocumentQuery<T>(this.Collection.DocumentsLink)
                    .Where(predicate)
                    .AsEnumerable()
                    .FirstOrDefault();
        }
    }
}