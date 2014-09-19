using Newtonsoft.Json;

namespace BouvetCodeCamp.Felles.Entiteter
{
    public class Kode
    {
        [JsonProperty(PropertyName = "gps")]
        public Coordinate Gps { get; set; }

        [JsonProperty(PropertyName = "bokstav")]
        public string Bokstav { get; set; }

        [JsonProperty(PropertyName = "posisjonTilstand")]
        public PosisjonTilstand PosisjonTilstand { get; set; }

        public Kode()
        {
            Gps = Coordinate.Empty;
            Bokstav = string.Empty;
            PosisjonTilstand = PosisjonTilstand.Ukjent;
        }
    }
}