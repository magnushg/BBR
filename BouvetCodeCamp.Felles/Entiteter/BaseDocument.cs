using Newtonsoft.Json;

namespace BouvetCodeCamp.Felles.Entiteter
{
    using System.ComponentModel;

    public abstract class BaseDocument
    {
        [JsonProperty(PropertyName = "id")]
        [DisplayName("Id")]
        public string DocumentId { get; set; }

        protected BaseDocument()
        {
            this.DocumentId = string.Empty;
        }
    }
}