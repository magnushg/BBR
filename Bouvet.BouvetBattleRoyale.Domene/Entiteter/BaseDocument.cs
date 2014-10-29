namespace Bouvet.BouvetBattleRoyale.Domene.Entiteter
{
    using System;
    using System.ComponentModel;

    using Newtonsoft.Json;

    [Serializable]
    public abstract class BaseDocument
    {
        [JsonProperty(PropertyName = "id")]
        [DisplayName("Id")]
        public string DocumentId { get; set; }

        [JsonProperty(PropertyName = "_self")]
        public string SelfLink { get; set; }

        [JsonProperty(PropertyName = "_etag")]
        public string Etag { get; set; }

        protected BaseDocument()
        {
            DocumentId = string.Empty;
            SelfLink = string.Empty;
            Etag = string.Empty;
        }
    }
}