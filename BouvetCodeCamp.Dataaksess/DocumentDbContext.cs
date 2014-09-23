using System;
using System.Linq;
using System.Threading.Tasks;
using BouvetCodeCamp.Dataaksess.Interfaces;
using BouvetCodeCamp.DomeneTjenester;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace BouvetCodeCamp.Dataaksess
{
    public class DocumentDbContext : IDocumentDbContext
    {
        private readonly IKonfigurasjon _konfigurasjon;

        private Database _database;

        public Database Database
        {
            get
            {
                if (_database == null)
                {
                    ReadOrCreateDatabase().Wait();
                }

                return _database;
            }
        }
        
        private string _databaseId;

        public String DatabaseId
        {
            get
            {
                if (string.IsNullOrEmpty(_databaseId))
                {
                    _databaseId = _konfigurasjon.HentAppSetting(DocumentDbKonstanter.DatabaseId);
                }

                return _databaseId;
            }
        }

        private DocumentClient _client;

        public DocumentClient Client
        {
            get
            {
                if (_client == null)
                {
                    var endpoint = _konfigurasjon.HentAppSetting(DocumentDbKonstanter.Endpoint);
                    var authKey = _konfigurasjon.HentAppSetting(DocumentDbKonstanter.AuthKey);
                    
                    var endpointUri = new Uri(endpoint);

                    _client = new DocumentClient(endpointUri, authKey);
                }

                return _client;
            }
        }

        public DocumentDbContext(IKonfigurasjon konfigurasjon)
        {
            _konfigurasjon = konfigurasjon;
        }

        public async Task ReadOrCreateDatabase()
        {
            var databases = Client.CreateDatabaseQuery()
                            .Where(db => db.Id == DatabaseId).ToArray();

            if (databases.Any())
            {
                _database = databases.First();
            }
            else
            {
                var database = new Database { Id = DatabaseId };
                _database = await Client.CreateDatabaseAsync(database);
            }
        }

        public DocumentCollection ReadOrCreateCollection(string databaseLink, string collectionId)
        {
            var collections = Client.CreateDocumentCollectionQuery(databaseLink)
                              .Where(col => col.Id == collectionId).ToArray();

            if (collections.Any())
            {
                return collections.First();
            }

            return Client.CreateDocumentCollectionAsync(databaseLink, new DocumentCollection { Id = collectionId }).Result;
        }
    }
}