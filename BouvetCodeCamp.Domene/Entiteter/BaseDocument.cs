using System.ComponentModel;
using Newtonsoft.Json;

namespace BouvetCodeCamp.Domene.Entiteter
{
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