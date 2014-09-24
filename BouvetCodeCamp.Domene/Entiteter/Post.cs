using System;
using System.Security.Policy;
using Newtonsoft.Json;

namespace BouvetCodeCamp.Domene.Entiteter
{
    public class Post : BaseDocument
    {
        [JsonProperty(PropertyName = "navn")]
        public string Navn { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public string Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public string Longitude { get; set; }

        [JsonProperty(PropertyName = "altitude")]
        public double? Altitude { get; set; }

        [JsonProperty(PropertyName = "bilde")]
        public string Bilde { get; set; }

        [JsonProperty(PropertyName = "beskrivelse")]
        public string Beskrivelse { get; set; }

        [JsonProperty(PropertyName = "kilde")]
        public string Kilde { get; set; }

        public Post()
        {
            Latitude = string.Empty;
            Longitude = string.Empty;
            Altitude = -999;
            Bilde = string.Empty;
            Beskrivelse = string.Empty;
            Kilde = string.Empty;
            Navn = string.Empty;
        }
    }
}