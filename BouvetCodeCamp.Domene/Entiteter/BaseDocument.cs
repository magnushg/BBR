using System.ComponentModel;
using Newtonsoft.Json;

namespace BouvetCodeCamp.Domene.Entiteter
{
    using System;

    [Serializable]
    public abstract class BaseDocument
    {
        [JsonProperty(PropertyName = "id")]
        [DisplayName("Id")]
        public string DocumentId { get; set; }

        [JsonProperty(PropertyName = "_self")]
        public string SelfLink { get; set; }

        protected BaseDocument()
        {
            DocumentId = string.Empty;
            SelfLink = string.Empty;
        }
    }
}