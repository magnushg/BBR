namespace Bouvet.BouvetBattleRoyale.Domene.Entiteter
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    using Newtonsoft.Json;

    public class Koordinat
    {
        //http://stackoverflow.com/a/18690202/1770699
        private static readonly Regex GyldigKoordinatRegex = new Regex(@"^[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)$");

        [JsonProperty(PropertyName = "longitude")]
        public string Longitude { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public string Latitude { get; set; }

        public double X
        {
            get
            {
                double verdi = 0;

                double.TryParse(Longitude, NumberStyles.Float, new NumberFormatInfo(), out verdi);

                return verdi;
            }
        }

        public double Y
        {
            get
            {
                double verdi = 0;

                double.TryParse(Latitude, NumberStyles.Float, new NumberFormatInfo(), out verdi);

                return verdi;
            }
        }

        public Koordinat()
        {
        }

        public Koordinat(string enStreng)
            : this(enStreng.Split(',')[0], enStreng.Split(',')[1])
        {
        }

        public Koordinat(string longitude, string latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public Koordinat(double longitude, double latitude)
        {
            Longitude = longitude.ToString(CultureInfo.InvariantCulture);
            Latitude = latitude.ToString(CultureInfo.InvariantCulture);
        }

        public static Koordinat Empty
        {
            get
            {
                return new Koordinat("0", "0");
            }
        }

        public static Koordinat ParseKoordinat(string koordinat)
        {
            if (!ErStringEtGyldigKoordinat(koordinat))
                throw new ArgumentException("Ugyldig koordinat");

            var koordinater = koordinat.Trim().Split(',');

            return new Koordinat(koordinater[0], koordinater[1]);
        }

        public static bool ErStringEtGyldigKoordinat(string koordinat)
        {
            return GyldigKoordinatRegex.IsMatch(koordinat);
        }
    }
}