using System;
using System.Linq;
using System.Threading.Tasks;
using BouvetCodeCamp.Felles.Konfigurasjon;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;

namespace BouvetCodeCamp.Dataaksess.Repositories
{
    using System.Configuration;

    using Felles;

    using Microsoft.Azure.Documents.Client;

    public abstract class BaseRepository
    {
        private readonly IKonfigurasjon _konfigurasjon;

        private Database _database;
        protected Database Database
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
        private String DatabaseId
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
        protected DocumentClient Client
        {
            get
            {
                if (_client == null)
                {
                    String endpoint = _konfigurasjon.HentAppSetting(DocumentDbKonstanter.Endpoint);
                    string authKey = _konfigurasjon.HentAppSetting(DocumentDbKonstanter.AuthKey);
                    Uri endpointUri = new Uri(endpoint);
                    _client = new DocumentClient(endpointUri, authKey);
                }

                return _client;
            }
        }

        protected BaseRepository(IKonfigurasjon konfigurasjon)
        {
            _konfigurasjon = konfigurasjon;
        }

        private async Task ReadOrCreateDatabase()
        {
            var databases = Client.CreateDatabaseQuery()
                            .Where(db => db.Id == DatabaseId).ToArray();

            if (databases.Any())
            {
                _database = databases.First();
            }
            else
            {
                Database database = new Database { Id = DatabaseId };
                database = await Client.CreateDatabaseAsync(database);
            }
        }

        protected abstract Task ReadOrCreateCollection(string databaseLink);
    }
}