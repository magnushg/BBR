namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Data.Interfaces
{
    using System.Threading.Tasks;

    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;

    public interface IDocumentDbContext
    {
        Task ReadOrCreateDatabase();
        DocumentCollection ReadOrCreateCollection(string databaseLink, string collectionId);
        Database Database { get; }
        string DatabaseId { get; }
        DocumentClient Client { get; }
    }
}