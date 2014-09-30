namespace BouvetCodeCamp.Domene.OutputModels
{
    using BouvetCodeCamp.Domene.Entiteter;

    using Newtonsoft.Json;

    public class KodeOutputModel
    {
        [JsonProperty(PropertyName = "kode")]
        public string Kode { get; set; }

        [JsonProperty(PropertyName = "koordinat")]
        public Koordinat Koordinat { get; set; }
    }
}