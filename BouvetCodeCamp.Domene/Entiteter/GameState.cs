using System.ComponentModel;
using Newtonsoft.Json;

namespace BouvetCodeCamp.Domene.Entiteter
{
    public class GameState : BaseDocument
    {
        [JsonProperty(PropertyName = "infisertPolygon")]
        [DisplayName("InfisertPolygon")]
        public InfisertPolygon InfisertPolygon { get; set; }
    }
}
