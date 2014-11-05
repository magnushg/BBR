namespace Bouvet.BouvetBattleRoyale.Domene.Entiteter
{
    using System;

    using Newtonsoft.Json;

    public class LoggHendelse : BaseDocument
    {
        [JsonProperty(PropertyName = "hendelsesType")]
        public HendelseType HendelseType { get; set; }

        [JsonProperty(PropertyName = "tid")]
        public DateTime Tid { get; set; }

        [JsonProperty(PropertyName = "kommentar")]
        public string Kommentar { get; set; }

        [JsonProperty(PropertyName = "lagId")]
        public string LagId { get; set; }

        public LoggHendelse()
        {
            LagId = string.Empty;
            HendelseType = HendelseType.Ukjent;
            Tid = DateTime.MinValue;
            Kommentar = string.Empty;
        }
    }
}