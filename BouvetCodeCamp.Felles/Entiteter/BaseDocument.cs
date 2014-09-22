using Newtonsoft.Json;

namespace BouvetCodeCamp.Felles.Entiteter
{
    public abstract class BaseDocument
    {
        [JsonProperty(PropertyName = "id")]
        public string DocumentId { get; private set; }

        protected BaseDocument()
        {
            this.DocumentId = string.Empty;
        }
    }
}