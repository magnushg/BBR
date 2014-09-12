namespace BouvetCodeCamp.Dataaksess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;

    using Interfaces;
    using Felles;
    using Felles.Entiteter;

    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;

    using Newtonsoft.Json;

    public class PifPosisjonRepository : BaseRepository, IPifPosisjonRepository
    {
        public PifPosisjonRepository()
        {
            CollectionId = ConfigurationManager.AppSettings[DocumentDbKonstanter.PifPosisjonerCollectionId]; 
        }

        public async Task Opprett(PifPosisjon pifPosisjon)
        {
            using (Client = new DocumentClient(new Uri(DocumentDbKonstanter.Endpoint), DocumentDbKonstanter.AuthKey))
            {
                var database = await DocumentDbHelpers.HentEllerOpprettDatabaseAsync(Client, DatabaseId);

                var collection = await DocumentDbHelpers.HentEllerOpprettCollectionAsync(Client, database.SelfLink, CollectionId);

                await Client.CreateDocumentAsync(collection.SelfLink, pifPosisjon);
            }
        }

        public async Task<IEnumerable<PifPosisjon>> HentAlle()
        {
            if (string.IsNullOrEmpty(DatabaseId))
                throw new ConfigurationErrorsException("db config missing");

            var allePifPosisjoner = new List<PifPosisjon>();

            using (Client = new DocumentClient(new Uri(DocumentDbKonstanter.Endpoint), DocumentDbKonstanter.AuthKey))
            {
                var database = await DocumentDbHelpers.HentEllerOpprettDatabaseAsync(Client, DatabaseId);

                var collection = await DocumentDbHelpers.HentEllerOpprettCollectionAsync(Client, database.SelfLink, CollectionId);

                var allePifPosisjonerQuery = Client.CreateDocumentQuery(collection.DocumentsLink, "SELECT p.LagId, p.Latitude, p.Longitude FROM " + CollectionId + " p").ToList();

                foreach (var pifPosisjon in allePifPosisjonerQuery)
                {
                    var konvertertFraJson = await JsonConvert.DeserializeObjectAsync<PifPosisjon>(pifPosisjon.ToString());

                    allePifPosisjoner.Add(konvertertFraJson);
                }
            }

            return allePifPosisjoner;
        }

        public async Task<IEnumerable<PifPosisjon>> HentPifPosisjon(string lagId)
        {
            if (string.IsNullOrEmpty(DatabaseId))
                throw new ConfigurationErrorsException("db config missing");

            var allePifPosisjoner = new List<PifPosisjon>();

            using (Client = new DocumentClient(new Uri(DocumentDbKonstanter.Endpoint), DocumentDbKonstanter.AuthKey))
            {
                var database = await DocumentDbHelpers.HentEllerOpprettDatabaseAsync(Client, DatabaseId);

                var collection = await DocumentDbHelpers.HentEllerOpprettCollectionAsync(Client, database.SelfLink, CollectionId);

                var allePifPosisjonerQuery = Client.CreateDocumentQuery(collection.DocumentsLink, "SELECT p.LagId, p.Latitude, p.Longitude FROM " + CollectionId + " p WHERE p.LagId LIKE '" + lagId + "'").ToList();

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