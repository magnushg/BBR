namespace BouvetCodeCamp.Dataaksess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;

    using BouvetCodeCamp.Dataaksess.Interfaces;
    using BouvetCodeCamp.Felles;
    using BouvetCodeCamp.Felles.Entiteter;

    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;

    using Newtonsoft.Json;

    public class AktivitetsloggRepository : BaseRepository, IAktivitetsloggRepository
    {
        public AktivitetsloggRepository()
        {
            this.CollectionId = ConfigurationManager.AppSettings[DocumentDbKonstanter.AktivitetsloggEntriesCollectionId];
        }

        public async Task Opprett(AktivitetsloggEntry aktivitetsloggEntry)
        {
            using (this.Client = new DocumentClient(new Uri(DocumentDbKonstanter.Endpoint), DocumentDbKonstanter.AuthKey))
            {
                var database = await DocumentDbHelpers.HentEllerOpprettDatabaseAsync(this.Client, this.DatabaseId);

                var collection = await DocumentDbHelpers.HentEllerOpprettCollectionAsync(this.Client, database.SelfLink, this.CollectionId);

                await this.Client.CreateDocumentAsync(collection.SelfLink, aktivitetsloggEntry);
            }
        }

        public async Task<IEnumerable<AktivitetsloggEntry>> HentAlle()
        {
            var alleAktivitetsloggEntries = new List<AktivitetsloggEntry>();

            using (this.Client = new DocumentClient(new Uri(DocumentDbKonstanter.Endpoint), DocumentDbKonstanter.AuthKey))
            {
                var database = await DocumentDbHelpers.HentEllerOpprettDatabaseAsync(this.Client, this.DatabaseId);

                var collection = await DocumentDbHelpers.HentEllerOpprettCollectionAsync(this.Client, database.SelfLink, this.CollectionId);

                var alleAktivitetsloggEntriesQuery = this.Client.CreateDocumentQuery(collection.DocumentsLink, "SELECT a.LagId, a.HendelsesType, a.Tid FROM " + this.CollectionId + " a").ToList();

                foreach (var aktivitetsloggEntry in alleAktivitetsloggEntriesQuery)
                {
                    var konvertertFraJson = await JsonConvert.DeserializeObjectAsync<AktivitetsloggEntry>(aktivitetsloggEntry.ToString());

                    alleAktivitetsloggEntries.Add(konvertertFraJson);
                }
            }

            return alleAktivitetsloggEntries;
        }
    }
}