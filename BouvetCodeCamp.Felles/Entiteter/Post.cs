using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BouvetCodeCamp.Felles.Entiteter
{
    public class Post : BaseDocument
    {
        public string Id { get; set; }
        public DateTime OpprettetDato { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Altitude { get; set; }
        public Url Bilde { get; set; }
        public string Kommentar { get; set; }
    }
}
