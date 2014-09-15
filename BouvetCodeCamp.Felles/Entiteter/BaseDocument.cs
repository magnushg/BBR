using Newtonsoft.Json;

namespace BouvetCodeCamp.Felles.Entiteter
{
    public abstract class BaseDocument
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        protected BaseDocument()
        {
            Id = string.Empty;
        }
    }
}