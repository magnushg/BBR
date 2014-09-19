using Newtonsoft.Json;

namespace BouvetCodeCamp.Felles.Entiteter
{
    using System.Collections.Generic;

    public class Lag : BaseDocument
    {
        [JsonProperty(PropertyName = "lagId")]
        public string LagId { get; set; }

        [JsonProperty(PropertyName = "poeng")]
        public int Poeng { get; set; }
        
        [JsonProperty(PropertyName = "pifPosisjoner")]
        public List<PifPosisjon> PifPosisjoner { get; set; }
        
        [JsonProperty(PropertyName = "koder")]
        public List<Kode> Koder { get; set; }

        [JsonProperty(PropertyName = "meldig")]
        public List<Melding> Melding { get; set; }

        [JsonProperty(PropertyName = "aktivitetsloggHendelser")]
        public List<AktivitetsloggHendelse> AktivitetsloggHendelser { get; set; }

        public Lag()
        {
            LagId = string.Empty;
            Poeng = 0;
            PifPosisjoner = new List<PifPosisjon>();
            Koder = new List<Kode>();
            Melding = new List<Melding>();
            AktivitetsloggHendelser = new List<AktivitetsloggHendelse>();
        }
    }
}