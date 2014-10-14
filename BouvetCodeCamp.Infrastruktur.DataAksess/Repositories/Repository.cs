namespace BouvetCodeCamp.Infrastruktur.DataAksess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using Domene.Entiteter;
    using DomeneTjenester.Interfaces;
    using Interfaces;

    using log4net;

    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;

    public abstract class Repository<T> : IRepository<T> where T : BaseDocument
    {
        public abstract string CollectionId { get; }
        
        protected readonly IKonfigurasjon _konfigurasjon;
        protected readonly IDocumentDbContext Context;

        private DocumentCollection _collection;

        private ILog log;

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
            
            log = LogManager.GetLogger(typeof(Repository<T>));
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
            try
            {
                var oppdaterStart = DateTime.Now;

                log.Info("Oppdaterer " + document.DocumentId);

                var entitet = Context.Client.CreateDocumentQuery<Document>(Collection.DocumentsLink)
                    .Where(d => d.Id == document.DocumentId)
                    .AsEnumerable()
                    .FirstOrDefault();

                if (entitet == null)
                    throw new Exception("Fant ikke entiteten som skulle oppdateres.");

                await Context.Client.ReplaceDocumentAsync(entitet.SelfLink, document);

                var oppdaterEnd = DateTime.Now;

                log.Info("Oppdatering på " + document.DocumentId + " tok..." + oppdaterStart.Subtract(oppdaterEnd));
            }
            catch (Exception e)
            {
                log.Error("Feil skjedde under oppdatering: " + e.Message);
                throw;
            }
        }

        public async Task Slett(T document)
        {
            var slettStart = DateTime.Now;
            log.Info("Sletter " + document.DocumentId);

            var entitet = Context.Client.CreateDocumentQuery<Document>(Collection.DocumentsLink)
                .Where(d => d.Id == document.DocumentId)
                .AsEnumerable()
                .FirstOrDefault();

            if (entitet == null)
                throw new Exception("Fant ikke entiteten som skulle slettes.");

            await Context.Client.DeleteDocumentAsync(entitet.SelfLink, new RequestOptions());

            var slettEnd = DateTime.Now;
            log.Info("Sletting av " + document.DocumentId + " tok..." + slettStart.Subtract(slettEnd));

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

        public IEnumerable<T> Søk(Func<T, bool> predicate)
        {
            return Context.Client.CreateDocumentQuery<T>(Collection.DocumentsLink)
                    .Where(predicate)
                    .AsEnumerable();
        }
    }
}