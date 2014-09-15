using System.ComponentModel;
using Newtonsoft.Json;

namespace BouvetCodeCamp.Felles.Entiteter
{
    using System;

    public class AktivitetsloggEntry
    {
        [JsonProperty(PropertyName = "lagId")]
        public int LagId { get; set; }

        [JsonProperty(PropertyName = "hendelsesType")]
        public HendelseType HendelseType { get; set; }

        [JsonProperty(PropertyName = "tid")]
        public DateTime Tid { get; set; }

        public AktivitetsloggEntry()
        {
            LagId = 0;
            HendelseType = HendelseType.Ukjent;
            Tid = DateTime.MinValue;
        }
    }
}