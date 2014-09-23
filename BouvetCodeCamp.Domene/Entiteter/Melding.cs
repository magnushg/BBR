using System;
using Newtonsoft.Json;

namespace BouvetCodeCamp.Domene.Entiteter
{
    public class Melding
    {
        [JsonProperty(PropertyName = "lagId")]
        public string LagId { get; set; }

        [JsonProperty(PropertyName = "tid")]
        public DateTime Tid { get; set; }

        [JsonProperty(PropertyName = "type")]
        public MeldingType Type { get; set; }

        [JsonProperty(PropertyName = "tekst")]
        public string Tekst { get; set; }

        public Melding()
        {
            LagId = string.Empty;
            Tid = DateTime.MinValue;
            Type = MeldingType.Ukjent;
            Tekst = string.Empty;
        }
    }
}