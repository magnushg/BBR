using Newtonsoft.Json;

namespace BouvetCodeCamp.Domene.Entiteter
{
    public class InfisertPolygon
    {
        [JsonProperty(PropertyName = "coordinates")]
        public Koordinat[] Koordinats { get; set; }
    }
}
