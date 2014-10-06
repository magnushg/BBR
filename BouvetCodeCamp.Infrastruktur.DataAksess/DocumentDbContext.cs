namespace BouvetCodeCamp.Infrastruktur.DataAksess
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BouvetCodeCamp.Infrastruktur.DataAksess.Interfaces;

    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;

    public class DocumentDbContext : IDocumentDbContext
    {
        private readonly IKonfigurasjon _konfigurasjon;

        private Database _database;

        public Database Database
        {
            get
            {
                if (this._database == null)
                {
                    this.ReadOrCreateDatabase().Wait();
                }

                return this._database;
            }
        }
        
        private string _databaseId;

        public String DatabaseId
        {
            get
            {
                if (string.IsNullOrEmpty(this._databaseId))
                {
                    this._databaseId = this._konfigurasjon.HentAppSetting(DocumentDbKonstanter.DatabaseId);
                }

                return this._databaseId;
            }
        }

        private DocumentClient _client;

        public DocumentClient Client
        {
            get
            {
                if (this._client == null)
                {
                    var endpoint = this._konfigurasjon.HentAppSetting(DocumentDbKonstanter.Endpoint);
                    var authKey = this._konfigurasjon.HentAppSetting(DocumentDbKonstanter.AuthKey);
                    
                    var endpointUri = new Uri(endpoint);

                    this._client = new DocumentClient(endpointUri, authKey);
                }

                return this._client;
            }
        }

        public DocumentDbContext(IKonfigurasjon konfigurasjon)
        {
            this._konfigurasjon = konfigurasjon;
        }

        public async Task ReadOrCreateDatabase()
        {
            var databases = this.Client.CreateDatabaseQuery()
                            .Where(db => db.Id == this.DatabaseId).ToArray();

            if (databases.Any())
            {
                this._database = databases.First();
            }
            else
            {
                var database = new Database { Id = this.DatabaseId };
                this._database = await this.Client.CreateDatabaseAsync(database);
            }
        }

        public DocumentCollection ReadOrCreateCollection(string databaseLink, string collectionId)
        {
            var collections = this.Client.CreateDocumentCollectionQuery(databaseLink)
                              .Where(col => col.Id == collectionId).ToArray();

            if (collections.Any())
            {
                return collections.First();
            }

            return this.Client.CreateDocumentCollectionAsync(databaseLink, new DocumentCollection { Id = collectionId }).Result;
        }
    }
}