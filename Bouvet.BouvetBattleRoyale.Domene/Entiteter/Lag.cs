namespace Bouvet.BouvetBattleRoyale.Domene.Entiteter
{
    using System.Collections.Generic;
    using System.ComponentModel;

    using Newtonsoft.Json;

    public class Lag : BaseDocument
    {
        [JsonProperty(PropertyName = "lagId")]
        [DisplayName("LagId")]
        public string LagId { get; set; }

        [JsonProperty(PropertyName = "lagnummer")]
        [DisplayName("LagNummer")]
        public int LagNummer { get; set; }

        [JsonProperty(PropertyName = "lagNavn")]
        [DisplayName("lagNavn")]
        public string LagNavn { get; set; }

        [JsonProperty(PropertyName = "poeng")]
        [DisplayName("Poeng")]
        public int Poeng { get; set; }

        [JsonProperty(PropertyName = "pifPosisjoner")]
        [DisplayName("PifPosisjoner")]
        public List<PifPosisjon> PifPosisjoner { get; set; }

        [JsonProperty(PropertyName = "poster")]
        [DisplayName("Poster")]
        public List<LagPost> Poster { get; set; }

        [JsonProperty(PropertyName = "meldinger")]
        [DisplayName("Meldinger")]
        public List<Melding> Meldinger { get; set; }

        public Lag()
        {
            LagId = string.Empty;
            Poeng = 0;
            PifPosisjoner = new List<PifPosisjon>();
            Poster = new List<LagPost>();
            Meldinger = new List<Melding>();
        }
    }
}