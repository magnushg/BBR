using Newtonsoft.Json;

namespace BouvetCodeCamp.Felles.Entiteter
{
    using System;

    public class LoggHendelse : BaseDocument
    {
        [JsonProperty(PropertyName = "lagId")]
        public string LagId { get; set; }

        [JsonProperty(PropertyName = "hendelsesType")]
        public HendelseType HendelseType { get; set; }

        [JsonProperty(PropertyName = "tid")]
        public DateTime Tid { get; set; }

        public LoggHendelse()
        {
            LagId = string.Empty;
            HendelseType = HendelseType.Ukjent;
            Tid = DateTime.MinValue;
        }
    }
}