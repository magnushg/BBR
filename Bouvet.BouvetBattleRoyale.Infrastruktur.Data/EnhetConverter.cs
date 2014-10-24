namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Data
{
    using System;
    using System.Text;

    using Newtonsoft.Json;

    public static class EnhetConverter
    {
        public static double HentObjektStorrelse<T>(T document)
        {
            var documentSomJson = JsonConvert.SerializeObject(document);

            return ConvertBytesToKilebytes(Encoding.UTF8.GetByteCount(documentSomJson));
        }

        public static double ConvertBytesToKilebytes(int bytes)
        {
            return Math.Round((bytes / 1024f), 2);
        }
    }
}