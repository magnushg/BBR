using Newtonsoft.Json;

namespace BouvetCodeCamp.Domene.Entiteter
{
    public class InfisertPolygon
    {
        [JsonProperty(PropertyName = "koordinater")]
        public Koordinat[] Koordinater { get; set; }
    }
}
