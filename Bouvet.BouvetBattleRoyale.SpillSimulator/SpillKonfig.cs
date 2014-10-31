using System.Collections.Generic;

namespace BouvetCodeCamp.SpillSimulator
{
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Domene.OutputModels;

    public static class SpillKonfig
    {
        public const string Passord = "mysecret";

        public const string Brukernavn = "bouvet";

        public static string ApiBaseAddress = string.Empty;

        public static string LagId;

        public static List<Koordinat> Koordinater;

        public static int KoordinatIndex = 0;

        public static PostOutputModell GjeldendePost;

        public static Dictionary<string, Dictionary<int, string>> LagMedPostkoder;
    }
}
