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

    public class MeldingRepository : BaseRepository, IMeldingRepository
    {
        public MeldingRepository()
        {
            this.CollectionId = ConfigurationManager.AppSettings[DocumentDbKonstanter.MeldingerCollectionId]; 
        }

        public async Task Opprett(Melding entitet)
        {
            using (this.Client = new DocumentClient(new Uri(DocumentDbKonstanter.Endpoint), DocumentDbKonstanter.AuthKey))
            {
                var database = await DocumentDbHelpers.HentEllerOpprettDatabaseAsync(this.Client, this.DatabaseId);

                var collection = await DocumentDbHelpers.HentEllerOpprettCollectionAsync(this.Client, database.SelfLink, this.CollectionId);

                await this.Client.CreateDocumentAsync(collection.SelfLink, entitet);
            }
        }

        public async Task<IEnumerable<Melding>> HentAlle()
        {
            var alleMeldinger = new List<Melding>();

            using (this.Client = new DocumentClient(new Uri(DocumentDbKonstanter.Endpoint), DocumentDbKonstanter.AuthKey))
            {
                var database = await DocumentDbHelpers.HentEllerOpprettDatabaseAsync(this.Client, this.DatabaseId);

                var collection = await DocumentDbHelpers.HentEllerOpprettCollectionAsync(this.Client, database.SelfLink, this.CollectionId);

                var alleMeldingerQuery = this.Client.CreateDocumentQuery(collection.DocumentsLink, "SELECT m.LagId, m.Tid, m.Type, m.Tekst FROM " + this.CollectionId + " m").ToList();

                foreach (var melding in alleMeldingerQuery)
                {
                    var konvertertFraJson = await JsonConvert.DeserializeObjectAsync<Melding>(melding.ToString());

                    alleMeldinger.Add(konvertertFraJson);
                }
            }

            return alleMeldinger;
        }
    }
}