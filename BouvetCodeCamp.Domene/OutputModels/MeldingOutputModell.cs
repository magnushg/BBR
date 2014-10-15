namespace BouvetCodeCamp.Domene.OutputModels
{
    using System;

    using Newtonsoft.Json;

    public class MeldingOutputModell
    {
        [JsonProperty(PropertyName = "type")]
        public MeldingType Type { get; set; }

        [JsonProperty(PropertyName = "innhold")]
        public string Innhold { get; set; }

        [JsonProperty(PropertyName = "lagId")]
        public string LagId { get; set; }

        [JsonProperty(PropertyName = "tid")]
        public DateTime Tid { get; set; }
    }
}