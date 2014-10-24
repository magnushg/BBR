namespace Bouvet.BouvetBattleRoyale.Domene.Entiteter
{
    using BouvetCodeCamp.Domene.Entiteter;

    using Newtonsoft.Json;

    public class InfisertPolygon
    {
        [JsonProperty(PropertyName = "koordinater")]
        public Koordinat[] Koordinater { get; set; }
    }
}
