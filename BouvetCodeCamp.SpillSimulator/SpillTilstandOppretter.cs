using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BouvetCodeCamp.Domene.Entiteter;

namespace BouvetCodeCamp.SpillSimulator
{
    public class SpillTilstandOppretter
    {
        public SpillTilstandOppretter()
        {
            OpprettKoordinater();
            OpprettPostKoder();
        }

        private static void OpprettKoordinater()
        {
            SpillKonfig.Koordinater = new List<Koordinat>
            {
                new Koordinat
                {
                    Latitude = "59.67878",
                    Longitude = "10.60392"
                },
                new Koordinat
                {
                    Latitude = "59.67944",
                    Longitude = "10.6042"
                },
                new Koordinat
                {
                    Latitude = "59.68023411",
                    Longitude = "10.6041259971"
                },
                new Koordinat
                {
                    Latitude = "59.6804558114",
                    Longitude = "10.60457",
                },
                new Koordinat
                {
                    Latitude = "59.6735388134",
                    Longitude = "10.6051534508",
                },
                 new Koordinat
                {
                    Latitude = "59.67616",
                    Longitude = "10.60672",
                },
                 new Koordinat
                {
                    Latitude = "59.67775",
                    Longitude = "10.60778",
                },
                 new Koordinat
                {
                    Latitude = "59.67865",
                    Longitude = "10.60835",
                }

            };
        }

        private void OpprettPostKoder()
        {
            SpillKonfig.LagMedPostkoder = new Dictionary<string, Dictionary<int, string>>
            {
                {"175", new Dictionary<int, string> {
                                                    {24, "dxg19"},
                                                    {21, "exr16"},
                                                    {23, "rxp18"},
                                                    {1, "oxx2"},
                                                    {8, "lxr9"},
                                                    {13, "axne"},
                                                    {2, "fxo3" }
                                                }},
                {"126", new Dictionary<int, string>
                                                {
                                                    {13, "nxn11" },
                                                    {2, "gxn6" },
                                                    {28, "exy20" },
                                                    {24, "hxc1c" },
                                                    {19, "llxk17" },
                                                    {5, "fxf1a" },
                                                    {6, "uxff" }
                                                }},
               {"35", new Dictionary<int, string> {
                                                    {30, "kxv20"},
                                                    {19, "axx15"},
                                                    {20, "exw16"},
                                                    {29, "axo1f"},
                                                    {13, "dxdf"},
                                                    {9, "bxwb"},
                                                    {4, "lxc6" }
                                                }},
            };
        }}
}
