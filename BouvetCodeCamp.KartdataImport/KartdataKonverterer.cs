using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using BouvetCodeCamp.Domene.Entiteter;

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
                    GpsPunktId = StripVekkUgyldigeTegn(loc[0]),
                    OpprettetDato = DateTime.Parse(loc[1]),
                    Latitude = StripVekkUgyldigeTegn(loc[2]),
                    Longitude = StripVekkUgyldigeTegn(loc[3]),
                    Altitude = double.Parse(loc[4], CultureInfo.InvariantCulture),
                    Bilde = StripVekkUgyldigeTegn(loc[10]),
                    Kommentar = StripVekkUgyldigeTegn(loc[11])
                })
                .ToList();
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
}
