using BouvetCodeCamp.Domene.Entiteter;
using Newtonsoft.Json;

namespace BouvetCodeCamp.Domene.OutputModels
{
    public class PostOutputModell
    {
        [JsonProperty(PropertyName = "navn")]
        public string Navn { get; set; }

        [JsonProperty(PropertyName = "gps")]
        public Koordinat Posisjon { get; set; }

        [JsonProperty(PropertyName = "postNummer")]
        public int Nummer { get; set; }
    }
}
