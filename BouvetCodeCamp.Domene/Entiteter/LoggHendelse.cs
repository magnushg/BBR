using System;
using Newtonsoft.Json;

namespace BouvetCodeCamp.Domene.Entiteter
{
    public class LoggHendelse
    {
        [JsonProperty(PropertyName = "hendelsesType")]
        public HendelseType HendelseType { get; set; }

        [JsonProperty(PropertyName = "tid")]
        public DateTime Tid { get; set; }

        public string Kommentar { get; set; }

        public LoggHendelse()
        {
            HendelseType = HendelseType.Ukjent;
            Tid = DateTime.MinValue;
            Kommentar = string.Empty;
        }
    }
}