namespace BouvetCodeCamp.Infrastruktur.DataAksess
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;

    public class DocumentDbHelpers
    {
        public static async Task<DocumentCollection> HentEllerOpprettCollectionAsync(DocumentClient client, string dbLink, string id)
        {
            var collection = client.CreateDocumentCollectionQuery(dbLink).Where(c => c.Id == id).ToArray().FirstOrDefault();

            if (collection == null)
            {
                collection = await client.CreateDocumentCollectionAsync(dbLink, new DocumentCollection { Id = id });
            }

            return collection;
        }

        public static async Task SlettCollectionAsync(DocumentClient client, string collectionLink, string id)
        {
            if (collectionLink != null)
            {
                var collection = client.CreateDocumentCollectionQuery(collectionLink).Where(c => c.Id == id).ToArray().FirstOrDefault();

                if (collection != null)
                {
                    await client.DeleteDocumentCollectionAsync(collectionLink);
                }
            }
        }

        public static async Task<Database> HentEllerOpprettDatabaseAsync(DocumentClient client, string id)
        {
            var database = client.CreateDatabaseQuery().Where(db => db.Id == id).ToArray().FirstOrDefault();

            if (database == null)
            {
                database = await client.CreateDatabaseAsync(new Database { Id = id });
            }

            return database;
        }
        
        public static async Task SlettDatabaseAsync(DocumentClient client, string id)
        {
            var database = client.CreateDatabaseQuery().Where(db => db.Id == id).ToArray().FirstOrDefault();

            if (database != null)
            {
                await client.DeleteDatabaseAsync(database.SelfLink);
            }
        }
    }
}