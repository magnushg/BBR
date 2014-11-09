namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.CrossCutting;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.CrossCutting.Extensions;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Data.Interfaces;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Interfaces;

    using log4net;

    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;

    public abstract class Repository<T> : IRepository<T> where T : BaseDocument
    {
        private const int RequestLimitKb = 256;

        public abstract string CollectionId { get; }

        protected readonly IKonfigurasjon _konfigurasjon;

        protected readonly IDocumentDbContext Context;

        private readonly ILog _log;

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
        }

        public async Task<string> Opprett(T document)
        {
            var opprettStart = DateTime.Now;
            
            var opprettetDocument = await Context.Client.CreateDocumentAsync(Collection.SelfLink, document);

            var opprettEnd = DateTime.Now;

            LoggDbHandling("Oppretting", document, opprettStart, opprettEnd);

            return opprettetDocument.Resource.Id;
        }

        public IEnumerable<T> HentAlle()
        {
            var documents = Context.Client.CreateDocumentQuery<T>(Collection.DocumentsLink).AsEnumerable();

            return documents;
        }

        public T Hent(string id)
        {
            _log.Debug("Henter " + id);

            var document = Context.Client.CreateDocumentQuery<T>(Collection.DocumentsLink)
                .Where(d => d.DocumentId == id)
                .AsEnumerable()
                .FirstOrDefault();

            return document;
        }

        public async Task Oppdater(T document)
        {
            var oppdaterStart = DateTime.Now;

            var options = new RequestOptions
                              {
                                  AccessCondition = new AccessCondition
                                                        {
                                                            Type = AccessConditionType.IfMatch,
                                                            Condition = document.Etag
                                                        }
                              };

            await Context.Client.ReplaceDocumentAsync(document.SelfLink, document, options);
            
            var oppdaterEnd = DateTime.Now;

            LoggDbHandling("Oppdatering", document, oppdaterStart, oppdaterEnd);
        }

        public async Task Slett(T document)
        {
            var slettStart = DateTime.Now;

            await Context.Client.DeleteDocumentAsync(document.SelfLink, new RequestOptions());

            var slettEnd = DateTime.Now;

            LoggDbHandling("Sletting", document, slettStart, slettEnd);
        }

        public IEnumerable<T> Søk(Func<T, bool> predicate)
        {
            var documents = Context.Client.CreateDocumentQuery<T>(Collection.DocumentsLink)
                    .Where(predicate)
                    .AsEnumerable();

            return documents;
        }
        
        private void LoggDbHandling(string type, T document, DateTime start, DateTime end)
        {
            var documentStorrelse = EnhetConverter.HentObjektStorrelse(document);

            var varighet = start.Subtract(end).Duration();
            var varighetSomString = varighet.ToReadableString();

            string loggMelding = string.Format(
                "{0} av {1} på {2} kb tok... {3}",
                type,
                document.DocumentId,
                documentStorrelse,
                varighetSomString);

            if (varighet.TotalSeconds > 3)
                _log.Warn("Treg " + type.ToLower() + ", tok " + varighetSomString);

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