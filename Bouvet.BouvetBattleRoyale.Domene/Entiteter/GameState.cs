namespace Bouvet.BouvetBattleRoyale.Domene.Entiteter
{
    using System.ComponentModel;

    using BouvetCodeCamp.Domene.Entiteter;

    using Newtonsoft.Json;

    /// <summary>
    /// Denne klassen er for generelle globale states som ikke behøver
    /// sin egen kolleksjon
    /// </summary>
    public class GameState : BaseDocument
    {
        [JsonProperty(PropertyName = "infisertPolygon")]
        [DisplayName("InfisertPolygon")]
        public InfisertPolygon InfisertPolygon { get; set; }

        public GameState()
        {
            InfisertPolygon = new InfisertPolygon();
        }
    }
}