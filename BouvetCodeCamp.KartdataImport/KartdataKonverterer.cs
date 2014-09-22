using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using BouvetCodeCamp.Felles.Entiteter;

namespace BouvetCodeCamp.KartdataImport
{
    public class KartdataKonverterer
    {
        public IEnumerable<Post> KonverterKartdata()
        {
            var mapData = LesTekstFraFil("mapdata/oscarsborg.csv");
            return mapData
                .Split('\n')
                .Skip(1)
                .Select(x => x.Split(','))
                .Where(loc => loc.Count() >= 13)
                .Select(loc => new Post {
                                        Id = loc[0],
                                        OpprettetDato = DateTime.Parse(loc[1]),
                                        Latitude = loc[2],
                                        Longitude = loc[3],
                                        Altitude = double.Parse(loc[4], CultureInfo.InvariantCulture),
                                        Bilde = new Url(loc[10]),
                                        Kommentar = loc[11]
                                    }).ToList();
        }

        public string LesTekstFraFil(string filepath)
        {
            return File.ReadAllText(filepath, Encoding.UTF8);
        }
    }
}
