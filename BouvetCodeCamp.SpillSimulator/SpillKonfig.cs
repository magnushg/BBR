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

        public const string ApiBaseAddress = "http://bouvet-code-camp.azurewebsites.net/";

        public const string TestLagId = "175";

        public static List<Koordinat> Koordinater;

        public static int KoordinatIndex = 0;

        public static PostOutputModell GjeldendePost;

        public static Dictionary<int, string> PostKoder;
    }
}
