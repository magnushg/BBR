namespace BouvetCodeCamp.Felles
{
    using Newtonsoft.Json;

    public class Kode
    {
        [JsonProperty(PropertyName = "gps")]
        public string Gps { get; set; }

        [JsonProperty(PropertyName = "bokstav")]
        public string Bokstav { get; set; }

        [JsonProperty(PropertyName = "posisjonTilstand")]
        public PosisjonTilstand PosisjonTilstand { get; set; }

        public Kode()
        {
            this.Gps = string.Empty;
            this.Bokstav = string.Empty;
            this.PosisjonTilstand = PosisjonTilstand.Ukjent;
        }
    }
}