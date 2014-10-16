namespace BouvetCodeCamp.Domene.OutputModels
{
    using Newtonsoft.Json;

    public class LoggHendelseOutputModell
    {
        [JsonProperty(PropertyName = "lagnummer")]
        public int LagNummer { get; set; }

        [JsonProperty(PropertyName = "lagId")]
        public string LagId { get; set; }

        [JsonProperty(PropertyName = "tid")]
        public string Tid { get; set; }

        [JsonProperty(PropertyName = "hendelse")]
        public string Hendelse { get; set; }
        
        [JsonProperty(PropertyName = "kommentar")]
        public string Kommentar { get; set; }

        public LoggHendelseOutputModell()
        {
            LagNummer = 0;
            LagId = string.Empty;
            Tid = string.Empty;
            Hendelse = string.Empty;
            Kommentar = string.Empty;
        }
    }
}