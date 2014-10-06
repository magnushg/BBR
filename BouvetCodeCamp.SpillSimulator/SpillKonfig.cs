using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.Domene.OutputModels;

namespace BouvetCodeCamp.SpillSimulator
{
    public static class SpillKonfig
    {
        public const string Passord = "mysecret";

        public const string Brukernavn = "bouvet";

        public const string ApiBaseAddress = "http://localhost:2014";

        public const string TestLagId = "d1879d23-6907-4039-9b1f-774dbd2b1b8d";

        public static List<Koordinat> Koordinater;

        public static int KoordinatIndex = 0;

        public static PostOutputModell GjeldendePost;

        public static Dictionary<int, string> PostKoder;
    }
}
