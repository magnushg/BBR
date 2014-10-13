namespace BouvetCodeCamp.Domene.OutputModels
{
    using Newtonsoft.Json;

    public class PoengOutputModell
    {
        [JsonProperty(PropertyName = "lagId")]
        public string LagId { get; set; }
        
        [JsonProperty(PropertyName = "poeng")]
        public int NyPoengsum { get; set; }

        public PoengOutputModell()
        {
            LagId = string.Empty;
            NyPoengsum = 0;
        }
    }
}