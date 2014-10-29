namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Bouvet.BouvetBattleRoyale.Domene;
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Data.Interfaces;
    using Bouvet.BouvetBattleRoyale.Tjenester.Interfaces;

    using log4net;

    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;

    public class ConcurrencyHandler<T> where T : BaseDocument
    {
        private readonly List<KeyValuePair<string, string>> documentEtags;

        public ConcurrencyHandler()
        {
            documentEtags = new List<KeyValuePair<string, string>>();
        }

        public void SettEtagForDocument(string documentId, string etag)
        {
            lock (documentId)
            {
                var eksisterendeKeyValuePairsForDocument = documentEtags.FindAll(o => o.Key == documentId).ToList();

                SlettEksisterendeVerdierForDocument(eksisterendeKeyValuePairsForDocument);

                documentEtags.Add(new KeyValuePair<string, string>(documentId, etag));
            }
        }

        public void SettEtagForDocuments(IEnumerable<T> alleDocuments)
        {
            foreach (var document in alleDocuments)
            {
                if (document != null)
                    SettEtagForDocument(document.DocumentId, document.Etag);
            }
        }

        private void SlettEksisterendeVerdierForDocument(List<KeyValuePair<string, string>> eksisterendeKeyValuePairsForDocument)
        {
            if (eksisterendeKeyValuePairsForDocument.Any())
            {
                foreach (var eksisterendeKeyValuePair in eksisterendeKeyValuePairsForDocument)
                {
                    SlettEksisterendeVerdierForDocument(eksisterendeKeyValuePair.Key);
                }
            }
        }

        public void SlettEksisterendeVerdierForDocument(string documentId)
        {
            lock (documentId)
            {
                var keyValuePair = documentEtags.Find(o => o.Key == documentId);

                documentEtags.Remove(keyValuePair);
            }
        }

        public void VerifiserOppdatertEtagForDocument(string documentId, string etag)
        {
            var keyValuePair = documentEtags.Find(o => o.Key == documentId);

            if (keyValuePair.Value != etag)
                throw new ConcurrencyException(string.Format("Concurrency-feil: Dokumentet {0} har mismatch på ETag. Hent ny versjon av dokumentet og prøv igjen.", documentId));
        }
    }

    public abstract class Repository<T> : IRepository<T> where T : BaseDocument
    {
        private const int RequestLimitKb = 256;

        public abstract string CollectionId { get; }

        protected readonly IKonfigurasjon _konfigurasjon;

        protected readonly IDocumentDbContext Context;

        private readonly ILog _log;

        private static ConcurrencyHandler<T> concurrencyHandler;

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

        protected Repository(IKonfigurasjon konfigurasjon, IDocumentDbContext context, ILog log)
        {
            _konfigurasjon = konfigurasjon;
            Context = context;
            _log = log;

            concurrencyHandler = new ConcurrencyHandler<T>();
        }

        public async Task<string> Opprett(T document)
        {
            var opprettetDocument = await Context.Client.CreateDocumentAsync(Collection.SelfLink, document);

            concurrencyHandler.SettEtagForDocument(opprettetDocument.Resource.Id, opprettetDocument.Resource.ETag);

            return opprettetDocument.Resource.Id;
        }

        public IEnumerable<T> HentAlle()
        {
            var documents = Context.Client.CreateDocumentQuery<T>(Collection.DocumentsLink).AsEnumerable();

            concurrencyHandler.SettEtagForDocuments(documents);

            return documents;
        }

        public T Hent(string id)
        {
            _log.Debug("Henter " + id);

            var document = Context.Client.CreateDocumentQuery<T>(Collection.DocumentsLink)
                .Where(d => d.DocumentId == id)
                .AsEnumerable()
                .FirstOrDefault();

            if (document != null)
                concurrencyHandler.SettEtagForDocument(document.DocumentId, document.Etag);

            return document;
        }

        public async Task Oppdater(T document)
        {
            concurrencyHandler.VerifiserOppdatertEtagForDocument(document.DocumentId, document.Etag);

            var oppdaterStart = DateTime.Now;

            await Context.Client.ReplaceDocumentAsync(document.SelfLink, document);

            var oppdaterEnd = DateTime.Now;

            LoggOppdatering(document, oppdaterStart, oppdaterEnd);
        }

        public async Task Slett(T document)
        {
            concurrencyHandler.VerifiserOppdatertEtagForDocument(document.DocumentId, document.Etag);

            var slettStart = DateTime.Now;

            await Context.Client.DeleteDocumentAsync(document.SelfLink, new RequestOptions());

            concurrencyHandler.SlettEksisterendeVerdierForDocument(document.DocumentId);

            var slettEnd = DateTime.Now;

            LoggSletting(document, slettStart, slettEnd);
        }

        public IEnumerable<T> Søk(Func<T, bool> predicate)
        {
            var documents = Context.Client.CreateDocumentQuery<T>(Collection.DocumentsLink)
                    .Where(predicate)
                    .AsEnumerable();

            concurrencyHandler.SettEtagForDocuments(documents);

            return documents;
        }

        private void LoggOppdatering(T document, DateTime oppdaterStart, DateTime oppdaterEnd)
        {
            var documentStorrelse = EnhetConverter.HentObjektStorrelse(document);

            var deltaTid = oppdaterStart.Subtract(oppdaterEnd);

            string loggMelding = "Oppdatering av " + document.DocumentId + " på " + documentStorrelse + "kb tok..."
                                 + deltaTid.TotalSeconds + " sekunder";

            var oppdateringTidSomSekunder = deltaTid.Duration().TotalSeconds;

            if (oppdateringTidSomSekunder > 5)
                _log.Warn("Treg oppdatering, tok " + oppdateringTidSomSekunder);

            if (documentStorrelse > RequestLimitKb)
            {
                _log.Warn(loggMelding);
            }
            else
            {
                _log.Debug(loggMelding);
            }
        }

        private void LoggSletting(T document, DateTime slettStart, DateTime slettEnd)
        {
            var documentStorrelse = EnhetConverter.HentObjektStorrelse(document);

            var loggMelding = "Sletting av " + document.DocumentId + " på " + document + "kb tok..."
                              + slettStart.Subtract(slettEnd).TotalSeconds + " sekunder";

            if (documentStorrelse > RequestLimitKb)
            {
                _log.Warn(loggMelding);
            }
            else
            {
                _log.Debug(loggMelding);
            }
        }
    }
}