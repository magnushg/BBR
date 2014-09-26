using Newtonsoft.Json;

namespace BouvetCodeCamp.Domene.Entiteter
{
    public class InfisertPolygon
    {
        [JsonProperty(PropertyName = "coordinates")]
        public Coordinate[] Coordinates { get; set; }
    }
}
