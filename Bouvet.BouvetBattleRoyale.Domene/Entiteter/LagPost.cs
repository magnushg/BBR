namespace Bouvet.BouvetBattleRoyale.Domene.Entiteter
{
    using BouvetCodeCamp.Domene;
    using BouvetCodeCamp.Domene.Entiteter;

    using Newtonsoft.Json;

    public class LagPost : Post
    {
        [JsonProperty(PropertyName = "tilstand")]
        public PostTilstand PostTilstand { get; set; }

        [JsonProperty(PropertyName = "kode")]
        public string Kode { get; set; }

        [JsonProperty(PropertyName = "sekvensnummer")]
        public int Sekvensnummer { get; set; }
    }
}
