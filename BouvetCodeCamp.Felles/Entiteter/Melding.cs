﻿using Newtonsoft.Json;

namespace BouvetCodeCamp.Felles.Entiteter
{
    using System;

    public class Melding
    {
        [JsonProperty(PropertyName = "lagId")]
        public int LagId { get; set; }

        [JsonProperty(PropertyName = "tid")]
        public DateTime Tid { get; set; }

        [JsonProperty(PropertyName = "type")]
        public MeldingType Type { get; set; }

        [JsonProperty(PropertyName = "tekst")]
        public string Tekst { get; set; }

        public Melding()
        {
            LagId = 0;
            Tid = DateTime.MinValue;
            Type = MeldingType.Ukjent;
            Tekst = string.Empty;
        }
    }
}