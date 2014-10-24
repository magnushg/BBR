using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BouvetCodeCamp.Domene.Entiteter;

namespace BouvetCodeCamp.SpillSimulator
{
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;

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
                },
                 new Koordinat
                {
                    Latitude = "59.67865",
                    Longitude = "10.60535",
                },
                 new Koordinat
                {
                    Latitude = "59.67465",
                    Longitude = "10.60835",
                },
                 new Koordinat
                {
                    Latitude = "59.6735",
                    Longitude = "10.60335",
                }

            };
        }

        private void OpprettPostKoder()
        {
            SpillKonfig.LagMedPostkoder = new Dictionary<string, Dictionary<int, string>>
            {
                {"c0af3db", new Dictionary<int, string> {
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
                {"61", new Dictionary<int, string> {
                                                    {21, "pxg1b"},
                                                    {18, "fxd18"},
                                                    {24, "nxd1e"},
                                                    {15, "oxt15"},
                                                    {5, "ngxqb"},
                                                    {3, "ixa9"},
                                                    {10, "sxm10" }
                                                }},
                {"238", new Dictionary<int, string> {
                                                    {15, "axy12"},
                                                    {10, "lxud"},
                                                    {3, "nxy6"},
                                                    {7, "nxpa"},
                                                    {28, "dext1f"},
                                                    {30, "mxq21"},
                                                    {16, "uxx13" }
                                                }},
                {"55", new Dictionary<int, string> {
                                                    {16, "kxt18"},
                                                    {26, "txf22"},
                                                    {2, "mxma"},
                                                    {10, "mxm12"},
                                                    {19, "zxt1b"},
                                                    {21, "uxw1d"},
                                                    {28, "exj24" }
                                                }},
            };
        }}
}
