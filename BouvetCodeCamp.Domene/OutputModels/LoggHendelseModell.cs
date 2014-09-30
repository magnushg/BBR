﻿namespace BouvetCodeCamp.Domene.OutputModels
{
    using Newtonsoft.Json;

    public class LoggHendelseModell
    {
        [JsonProperty(PropertyName = "lagId")]
        public string LagId { get; set; }

        [JsonProperty(PropertyName = "tid")]
        public string Tid { get; set; }

        [JsonProperty(PropertyName = "hendelse")]
        public string Hendelse { get; set; }
    }
}