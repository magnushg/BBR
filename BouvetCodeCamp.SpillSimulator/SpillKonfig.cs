using System.Collections.Generic;

using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.Domene.OutputModels;

namespace BouvetCodeCamp.SpillSimulator
{
    public static class SpillKonfig
    {
        public const string Passord = "mysecret";

        public const string Brukernavn = "bouvet";

        public const string ApiBaseAddress = "http://bouvet-code-camp.azurewebsites.net/";

        public static string LagId;

        public static List<Koordinat> Koordinater;

        public static int KoordinatIndex = 0;

        public static PostOutputModell GjeldendePost;

        public static Dictionary<string, Dictionary<int, string>> LagMedPostkoder;
    }
}
