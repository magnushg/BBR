namespace Bouvet.BouvetBattleRoyale.Domene.InputModels
{
    using BouvetCodeCamp.Domene;

    using Newtonsoft.Json;

    public class LoggHendelseInputModell
    {
        [JsonProperty(PropertyName = "lagId")]
        public string LagId { get; set; }
        
        [JsonProperty(PropertyName = "hendelsetype")]
        public HendelseType HendelseType { get; set; }

        [JsonProperty(PropertyName = "kommentar")]
        public string Kommentar { get; set; }
    }
}