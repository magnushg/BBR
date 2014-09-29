using Newtonsoft.Json;

namespace BouvetCodeCamp.Domene.Entiteter
{
    public class Kode
    {
        [JsonProperty(PropertyName = "gps")]
        public Koordinat Posisjon { get; set; }

        [JsonProperty(PropertyName = "bokstav")]
        public string Bokstav { get; set; }

        [JsonProperty(PropertyName = "posisjonTilstand")]
        public PostTilstand PostTilstand { get; set; }

        public Kode()
        {
            Posisjon = Koordinat.Empty;
            Bokstav = string.Empty;
            PostTilstand = PostTilstand.Ukjent;
        }
    }
}