namespace BouvetCodeCamp.Infrastruktur.DataAksess.Repositories
{
    using System;
    using System.Collections.Generic;
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
        private const int RequestLimitKb = 256;

        public abstract string CollectionId { get; }

        protected readonly IKonfigurasjon _konfigurasjon;
        protected readonly IDocumentDbContext Context;

        private DocumentCollection _collection;

        private readonly ILog log;

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
            document = SorgForDocumentUnderRequestLimit(document);

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
            document = SorgForDocumentUnderRequestLimit(document);

            var oppdaterStart = DateTime.Now;
            
            await Context.Client.ReplaceDocumentAsync(document.SelfLink, document);

            var oppdaterEnd = DateTime.Now;

            var documentStorrelse = EnhetConverter.HentObjektStorrelse(document);

            if (documentStorrelse > RequestLimitKb)
                log.Warn(
                    "Oppdatering på " + document.DocumentId + " på " + documentStorrelse  + "kb tok..."
                    + oppdaterStart.Subtract(oppdaterEnd));
        }

        public async Task Slett(T document)
        {
            var slettStart = DateTime.Now;

            await Context.Client.DeleteDocumentAsync(document.SelfLink, new RequestOptions());

            var slettEnd = DateTime.Now;

            var documentStorrelse = EnhetConverter.HentObjektStorrelse(document);

            if (documentStorrelse > RequestLimitKb)
                log.Warn("Sletting av " + document.DocumentId + " på " + document + "kb tok..." + slettStart.Subtract(slettEnd));
        }

        public IEnumerable<T> Søk(Func<T, bool> predicate)
        {
            return Context.Client.CreateDocumentQuery<T>(Collection.DocumentsLink)
                    .Where(predicate)
                    .AsEnumerable();
        }

        private T SorgForDocumentUnderRequestLimit(T document)
        {
            var objektStorrelseKb = EnhetConverter.HentObjektStorrelse(document);

            if (objektStorrelseKb <= RequestLimitKb)
                return document;

            if (document is Lag)
            {
                var lag = (Lag)Convert.ChangeType(document, typeof(Lag));

                while (objektStorrelseKb >= RequestLimitKb)
                {
                    var loggHendelserTilSletting = lag.LoggHendelser.OrderBy(o => o.Tid).Take(200);

                    foreach (var loggHendelse in loggHendelserTilSletting)
                    {
                        lag.LoggHendelser.Remove(loggHendelse);
                    }
                    
                    var loggPifPosisjonerTilSletting = lag.PifPosisjoner.OrderBy(o => o.Tid).Take(200);

                    foreach (var pifPosisjoner in loggPifPosisjonerTilSletting)
                    {
                        lag.PifPosisjoner.Remove(pifPosisjoner);
                    }
                    
                    objektStorrelseKb = EnhetConverter.HentObjektStorrelse(lag);
                }

                log.Warn(string.Format("Krympet {0} ned til {1}kb", lag.DocumentId, objektStorrelseKb));

                document = (T)Convert.ChangeType(lag, typeof(T));
            }

            return document;
        }
    }
}