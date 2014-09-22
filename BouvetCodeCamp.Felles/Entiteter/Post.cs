using System;
using System.Security.Policy;

namespace BouvetCodeCamp.Felles.Entiteter
{
    public class Post : BaseDocument
    {
        public DateTime OpprettetDato { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public double? Altitude { get; set; }
        public Url Bilde { get; set; }
        public string Kommentar { get; set; }

        public Post()
        {
            OpprettetDato = DateTime.MinValue;
            Latitude = string.Empty;
            Longitude = string.Empty;
            Altitude = null;
            Bilde = null;
            Kommentar = string.Empty;
        }
    }
}
