namespace Bouvet.BouvetBattleRoyale.Domene.Entiteter
{
    using System;

    using BouvetCodeCamp.Domene;

    using Newtonsoft.Json;

    public class LoggHendelse
    {
        [JsonProperty(PropertyName = "hendelsesType")]
        public HendelseType HendelseType { get; set; }

        [JsonProperty(PropertyName = "tid")]
        public DateTime Tid { get; set; }

        [JsonProperty(PropertyName = "kommentar")]
        public string Kommentar { get; set; }

        public LoggHendelse()
        {
            HendelseType = HendelseType.Ukjent;
            Tid = DateTime.MinValue;
            Kommentar = string.Empty;
        }
    }
}