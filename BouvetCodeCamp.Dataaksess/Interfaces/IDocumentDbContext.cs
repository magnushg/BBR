using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace BouvetCodeCamp.Dataaksess.Interfaces
{
    public interface IDocumentDbContext
    {
        Task ReadOrCreateDatabase();
        DocumentCollection ReadOrCreateCollection(string databaseLink, string collectionId);
        Database Database { get; }
        string DatabaseId { get; }
        DocumentClient Client { get; }
    }
}