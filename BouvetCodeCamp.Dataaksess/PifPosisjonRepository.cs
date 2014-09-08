using System;
using System.Threading.Tasks;

namespace BouvetCodeCamp.Dataaksess
{
    using System.Collections.Generic;
    using System.Linq;

    using BouvetCodeCamp.Dataaksess.Interfaces;
    using BouvetCodeCamp.Felles;

    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;

    using Newtonsoft.Json;

    public class PifPosisjonRepository : IPifPosisjonRepository
    {
        DocumentClient client;

        private readonly string DatabaseId;

        private readonly string CollectionId;
        
        public PifPosisjonRepository(string databaseId, string collectionId)
        {
            DatabaseId = databaseId;
            CollectionId = collectionId;
        }

        public async Task Opprett(PifPosisjon pifPosisjon)
        {
            using (client = new DocumentClient(new Uri(Konstanter.DocumentDbEndpoint), Konstanter.DocumentDbAuthKey))
            {
                var database = await DocumentDbHelpers.HentEllerOpprettDatabaseAsync(client, DatabaseId);

                var collection = await DocumentDbHelpers.HentEllerOpprettCollectionAsync(client, database.SelfLink, CollectionId);

                await client.CreateDocumentAsync(collection.SelfLink, pifPosisjon);
            }
        }

        public async Task<IEnumerable<PifPosisjon>> HentAlle()
        {
            var allePifPosisjoner = new List<PifPosisjon>();

            using (client = new DocumentClient(new Uri(Konstanter.DocumentDbEndpoint), Konstanter.DocumentDbAuthKey))
            {
                var database = await DocumentDbHelpers.HentEllerOpprettDatabaseAsync(client, DatabaseId);

                var collection = await DocumentDbHelpers.HentEllerOpprettCollectionAsync(client, database.SelfLink, CollectionId);

                var allePifPosisjonerQuery = client.CreateDocumentQuery(collection.DocumentsLink, "SELECT p.LagId, p.Latitude, p.Longitude FROM " + CollectionId + " p").ToList();

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