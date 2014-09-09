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

    public class PifPosisjonRepository : BaseRepository, IPifPosisjonRepository
    {
        public PifPosisjonRepository()
        {
            this.CollectionId = ConfigurationManager.AppSettings[DocumentDbKonstanter.PifPosisjonerCollectionId]; 
        }

        public async Task Opprett(PifPosisjon pifPosisjon)
        {
            using (this.Client = new DocumentClient(new Uri(DocumentDbKonstanter.Endpoint), DocumentDbKonstanter.AuthKey))
            {
                var database = await DocumentDbHelpers.HentEllerOpprettDatabaseAsync(this.Client, this.DatabaseId);

                var collection = await DocumentDbHelpers.HentEllerOpprettCollectionAsync(this.Client, database.SelfLink, this.CollectionId);

                await this.Client.CreateDocumentAsync(collection.SelfLink, pifPosisjon);
            }
        }

        public async Task<IEnumerable<PifPosisjon>> HentAlle()
        {
            var allePifPosisjoner = new List<PifPosisjon>();

            using (this.Client = new DocumentClient(new Uri(DocumentDbKonstanter.Endpoint), DocumentDbKonstanter.AuthKey))
            {
                var database = await DocumentDbHelpers.HentEllerOpprettDatabaseAsync(this.Client, this.DatabaseId);

                var collection = await DocumentDbHelpers.HentEllerOpprettCollectionAsync(this.Client, database.SelfLink, this.CollectionId);

                var allePifPosisjonerQuery = this.Client.CreateDocumentQuery(collection.DocumentsLink, "SELECT p.LagId, p.Latitude, p.Longitude FROM " + this.CollectionId + " p").ToList();

                foreach (var pifPosisjon in allePifPosisjonerQuery)
                {
                    var konvertertFraJson = await JsonConvert.DeserializeObjectAsync<PifPosisjon>(pifPosisjon.ToString());

                    allePifPosisjoner.Add(konvertertFraJson);
                }
            }

            return allePifPosisjoner;
        }
    }
}