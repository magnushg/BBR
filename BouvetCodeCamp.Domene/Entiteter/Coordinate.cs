using System.Globalization;
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
        public Coordinate(double longitude, double latitude)
        {
            Longitude = longitude.ToString(CultureInfo.InvariantCulture);
            Latitude = latitude.ToString(CultureInfo.InvariantCulture);
        }

        public static Coordinate Empty
        {
            get
            {
                return new Coordinate("", "");
            }
        }

        public double X { get { return double.Parse(Longitude, CultureInfo.InvariantCulture); }}
        public double Y { get { return double.Parse(Latitude, CultureInfo.InvariantCulture); } }
    }
}
