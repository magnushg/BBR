using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace BouvetCodeCamp.Domene.Entiteter
{
    public class Lag : BaseDocument
    {
        [JsonProperty(PropertyName = "lagId")]
        [DisplayName("LagId")]
        public string LagId { get; set; }

        [JsonProperty(PropertyName = "poeng")]
        [DisplayName("Poeng")]
        public int Poeng { get; set; }

        [JsonProperty(PropertyName = "pifPosisjoner")]
        [DisplayName("PifPosisjoner")]
        public List<PifPosisjon> PifPosisjoner { get; set; }

        [JsonProperty(PropertyName = "koder")]
        [DisplayName("Koder")]
        public List<Kode> Koder { get; set; }

        [JsonProperty(PropertyName = "meldig")]
        [DisplayName("Meldinger")]
        public List<Melding> Meldinger { get; set; }

        [JsonProperty(PropertyName = "loggHendelser")]
        [DisplayName("loggHendelser")]
        public List<LoggHendelse> LoggHendelser { get; set; }

        public Lag()
        {
            LagId = string.Empty;
            Poeng = 0;
            PifPosisjoner = new List<PifPosisjon>();
            Koder = new List<Kode>();
            this.Meldinger = new List<Melding>();
            this.LoggHendelser = new List<LoggHendelse>();
        }
    }
}