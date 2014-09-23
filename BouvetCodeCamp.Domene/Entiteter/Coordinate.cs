using Newtonsoft.Json;

namespace BouvetCodeCamp.Domene.Entiteter
{
    public class Coordinate
    {
        [JsonProperty(PropertyName = "longitude")]
        public string Longitude { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public string Latitude { get; set; }
        public Coordinate()
        {

        }
        public Coordinate(string longitude, string latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public static Coordinate Empty
        {
            get
            {
                return new Coordinate("", "");
            }
        }
    }
}
