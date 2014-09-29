using Newtonsoft.Json;

namespace BouvetCodeCamp.Domene.Entiteter
{
    public class Post : BaseDocument
    {
        [JsonProperty(PropertyName = "navn")]
        public string Navn { get; set; }

        [JsonProperty(PropertyName = "gps")]
        public Koordinat Posisjon { get; set; }

        [JsonProperty(PropertyName = "altitude")]
        public double? Altitude { get; set; }

        [JsonProperty(PropertyName = "bilde")]
        public string Bilde { get; set; }

        [JsonProperty(PropertyName = "beskrivelse")]
        public string Beskrivelse { get; set; }

        [JsonProperty(PropertyName = "kilde")]
        public string Kilde { get; set; }

        [JsonProperty(PropertyName = "postNummer")]
        public int Nummer { get; set; }

        public Post()
        {
            Nummer = -1;
            Posisjon = Koordinat.Empty;
            Altitude = -999;
            Bilde = string.Empty;
            Beskrivelse = string.Empty;
            Kilde = string.Empty;
            Navn = string.Empty;
        }
    }
}