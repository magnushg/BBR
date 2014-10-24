namespace Bouvet.BouvetBattleRoyale.Domene.Entiteter
{
    using System;

    using Newtonsoft.Json;

    public class PifPosisjon
    {
        [JsonProperty(PropertyName = "lagId")]
        public string LagId { get; set; }

        [JsonProperty(PropertyName = "gps")]
        public Koordinat Posisjon { get; set; }

        [JsonProperty(PropertyName = "tid")]
        public DateTime Tid { get; set; }

        [JsonProperty(PropertyName = "infisert")]
        public bool Infisert { get; set; }



        public PifPosisjon()
        {
            LagId = string.Empty;
            Posisjon = Koordinat.Empty;
            Tid = DateTime.MinValue;
        }
    }
}