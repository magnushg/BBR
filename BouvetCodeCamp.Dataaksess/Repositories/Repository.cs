using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BouvetCodeCamp.Dataaksess.Interfaces;
using BouvetCodeCamp.Felles.Entiteter;
using BouvetCodeCamp.Felles.Konfigurasjon;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;

namespace BouvetCodeCamp.Dataaksess.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : BaseDocument
    {
        public virtual string CollectionId { get; private set; }
        public virtual DocumentCollection Collection { get; private set; }
        protected readonly IKonfigurasjon _konfigurasjon;
        protected readonly IDocumentDbContext Context;

        protected Repository(IKonfigurasjon konfigurasjon, IDocumentDbContext context)
        {
            _konfigurasjon = konfigurasjon;
            Context = context;
        }

        public async Task<Document> Opprett(T document)
        {
            return await Context.Client.CreateDocumentAsync(Collection.SelfLink, document);
        }

        public async Task<IEnumerable<T>> HentAlle()
        {
            return await Task.Run(() =>
                Context.Client.CreateDocumentQuery<T>(Collection.DocumentsLink)
                    .AsEnumerable()
                    .ToList());
        }

        public async Task<T> Hent(string id)
        {
            return await Task.Run(() =>
                Context.Client.CreateDocumentQuery<T>(Collection.DocumentsLink)
                    .Where(d => d.Id == id)
                    .AsEnumerable()
                    .FirstOrDefault());
        }

        public async Task Oppdater(T document)
        {
            var entitet = Context.Client.CreateDocumentQuery<Document>(Collection.DocumentsLink)
                .Where(d => d.Id == document.Id)
                .AsEnumerable().FirstOrDefault();

            if (entitet == null)
                throw new Exception("Fant ikke entiteten som skulle oppdateres.");

            await Context.Client.ReplaceDocumentAsync(entitet.SelfLink, document);
        }
    }
}