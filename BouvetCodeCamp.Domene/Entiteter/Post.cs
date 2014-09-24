using System;
using System.Security.Policy;
using Newtonsoft.Json;

namespace BouvetCodeCamp.Domene.Entiteter
{
    public class Post : BaseDocument
    {
        [JsonProperty(PropertyName = "gpsPunktId")]
        public string GpsPunktId { get; set; }

        [JsonProperty(PropertyName = "OpprettetDato")]
        public DateTime OpprettetDato { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public string Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public string Longitude { get; set; }

        [JsonProperty(PropertyName = "altitude")]
        public double? Altitude { get; set; }

        [JsonProperty(PropertyName = "bilde")]
        public string Bilde { get; set; }

        [JsonProperty(PropertyName = "kommentar")]
        public string Kommentar { get; set; }

        public Post()
        {
            GpsPunktId = string.Empty;
            OpprettetDato = DateTime.MinValue;
            Latitude = string.Empty;
            Longitude = string.Empty;
            Altitude = -999;
            Bilde = string.Empty;
            Kommentar = string.Empty;
        }
    }
}