namespace BouvetCodeCamp.Domene.OutputModels
{
    using Newtonsoft.Json;

    public class PostRegistrertOutputModell
    {
        [JsonProperty(PropertyName = "lagId")]
        public string LagId { get; set; }
        
        [JsonProperty(PropertyName = "postnummer")]
        public int Nummer { get; set; }

        public PostRegistrertOutputModell()
        {
            LagId = string.Empty;
            Nummer = 0;
        }
    }
}