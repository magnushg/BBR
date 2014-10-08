﻿using System;
using Newtonsoft.Json;

namespace BouvetCodeCamp.Domene.Entiteter
{
    public class PifPosisjon
    {
        [JsonProperty(PropertyName = "lagId")]
        public string LagId { get; set; }

        [JsonProperty(PropertyName = "gps")]
        public Koordinat Posisjon { get; set; }

        [JsonProperty(PropertyName = "tid")]
        public DateTime Tid { get; set; }



        public PifPosisjon()
        {
            LagId = string.Empty;
            Posisjon = Koordinat.Empty;
            Tid = DateTime.MinValue;
        }
    }
}