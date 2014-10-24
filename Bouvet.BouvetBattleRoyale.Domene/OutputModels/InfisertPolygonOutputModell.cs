namespace Bouvet.BouvetBattleRoyale.Domene.OutputModels
{
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;

    using Newtonsoft.Json;

    public class InfisertPolygonOutputModell
    {
        [JsonProperty(PropertyName = "koordinater")]
        public Koordinat[] Koordinater { get; set; }

        public InfisertPolygonOutputModell()
        {
            Koordinater = new Koordinat[0];
        }
    }
}