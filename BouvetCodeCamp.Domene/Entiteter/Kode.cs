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
        public PosisjonTilstand PosisjonTilstand { get; set; }

        public Kode()
        {
            Posisjon = Koordinat.Empty;
            Bokstav = string.Empty;
            PosisjonTilstand = PosisjonTilstand.Ukjent;
        }
    }
}