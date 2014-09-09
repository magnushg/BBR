namespace BouvetCodeCamp.Dataaksess.Repositories
{
    using System.Configuration;

    using BouvetCodeCamp.Felles;

    using Microsoft.Azure.Documents.Client;

    public abstract class BaseRepository
    {
        protected DocumentClient Client;

        protected readonly string DatabaseId;

        protected string CollectionId;

        protected BaseRepository()
        {
            this.DatabaseId = ConfigurationManager.AppSettings[DocumentDbKonstanter.DatabaseId];           
        }
    }
}