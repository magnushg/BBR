using Newtonsoft.Json;

namespace BouvetCodeCamp.Felles.Entiteter
{
    public class Lag : BaseDocument
    {
        [JsonProperty(PropertyName = "lagId")]
        public string LagId { get; set; }

        [JsonProperty(PropertyName = "poeng")]
        public int Poeng { get; set; }

        public Lag()
        {
            LagId = string.Empty;
            Poeng = 0;
        }
    }
}