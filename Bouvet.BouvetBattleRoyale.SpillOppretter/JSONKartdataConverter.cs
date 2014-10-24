using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using BouvetCodeCamp.Domene.Entiteter;
using Newtonsoft.Json;

namespace BouvetCodeCamp.SpillOppretter
{
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;

    public class JSONKartdataConverter
    {
        private readonly string _path;

        public JSONKartdataConverter(string path)
        {
            _path = path;
        }

        public IEnumerable<Post> KonverterKartdata()
        {
            var data = LesTekstFraFil(_path);

            var deserialisert = JsonConvert.DeserializeObject<IEnumerable<JPost>>(data);

            return deserialisert.Select((kartinfo, index) => new Post
            {
               Navn = StripVekkUgyldigeTegn(string.Format("Post {0}", index + 1)),
               Nummer = index + 1,
               Beskrivelse = StripVekkUgyldigeTegn(kartinfo.description),
               Bilde = StripVekkUgyldigeTegn(kartinfo.image.FirstOrDefault()),
               Posisjon = new Koordinat
               {
                   Latitude = StripVekkUgyldigeTegn(kartinfo.position.FirstOrDefault().latitude),
                   Longitude = StripVekkUgyldigeTegn(kartinfo.position.FirstOrDefault().longitude)
               },
               Altitude = kartinfo.position.FirstOrDefault().altitude,
               Kilde = StripVekkUgyldigeTegn(kartinfo.position.FirstOrDefault().source),
            });
        }

        public string LesTekstFraFil(string filepath)
        {
            return File.ReadAllText(filepath, Encoding.UTF8);
        }

        private string StripVekkUgyldigeTegn(string tekstMedUgyldigeTegn)
        {
            return tekstMedUgyldigeTegn.Replace("\"", "");
        }
    }

    public class JPost
    {
        public string description { get; set; }
        public JPosition[] position { get; set; }
        public string[] image { get; set; }
    }

    public class JPosition
    {
        public string source { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public double altitude { get; set; }
    }


}
