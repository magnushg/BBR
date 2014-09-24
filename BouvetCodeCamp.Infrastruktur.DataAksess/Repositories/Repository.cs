namespace BouvetCodeCamp.Infrastruktur.DataAksess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;
    using BouvetCodeCamp.Infrastruktur.DataAksess.Interfaces;

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
                if (this._collection == null)
                {
                    this._collection = this.Context.ReadOrCreateCollection(this.Context.Database.SelfLink, this.CollectionId);
                }

                return this._collection;
            }
        }

        protected Repository(IKonfigurasjon konfigurasjon, IDocumentDbContext context)
        {
            this._konfigurasjon = konfigurasjon;
            this.Context = context;
        }

        public async Task<string> Opprett(T document)
        {
            var opprettetDocument = await this.Context.Client.CreateDocumentAsync(this.Collection.SelfLink, document);

            return opprettetDocument.Resource.Id;
        }

        public async Task<IEnumerable<T>> HentAlle()
        {
            var aa = this.Context.Client.CreateDocumentQuery(
                this.Collection.DocumentsLink,
                "SELECT * FROM Post ").ToList();

            return await Task.Run(() =>
                this.Context.Client.CreateDocumentQuery<T>(this.Collection.DocumentsLink)
                    .AsEnumerable()
                    .ToList());
        }

        public async Task<T> Hent(string id)
        {
            return await Task.Run(() =>
                this.Context.Client.CreateDocumentQuery<T>(this.Collection.DocumentsLink)
                    .Where(d => d.DocumentId == id)
                    .AsEnumerable()
                    .FirstOrDefault());
        }

        public async Task Oppdater(T document)
        {
            var entitet = this.Context.Client.CreateDocumentQuery<Document>(this.Collection.DocumentsLink)
                .Where(d => d.Id == document.DocumentId)
                .AsEnumerable()
                .FirstOrDefault();

            if (entitet == null)
                throw new Exception("Fant ikke entiteten som skulle oppdateres.");

            await this.Context.Client.ReplaceDocumentAsync(entitet.SelfLink, document);
        }

        public async Task Slett(T document)
        {
            var entitet = this.Context.Client.CreateDocumentQuery<Document>(this.Collection.DocumentsLink)
                .Where(d => d.Id == document.DocumentId)
                .AsEnumerable()
                .FirstOrDefault();

            if (entitet == null)
                throw new Exception("Fant ikke entiteten som skulle slettes.");

            await this.Context.Client.DeleteDocumentAsync(entitet.SelfLink, new RequestOptions());
        }
    }
}