using System;
using System.Linq;

using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester.Interfaces;

namespace BouvetCodeCamp.DomeneTjenester
{
    public class KoordinatVerifier : IKoordinatVerifier
    {
        public static double LongProximityThreshold = 10;
        public static double LatProximityThreshold = 10;
        
        public bool KoordinaterErNærHverandre(Koordinat first, Koordinat second)
        {
            double firstLong, firstLat,
                secondLong, secondLat;

            Double.TryParse(first.Latitude, out firstLat);
            Double.TryParse(first.Longitude, out firstLong);
            Double.TryParse(second.Latitude, out secondLat);
            Double.TryParse(second.Longitude, out secondLong);

            return Math.Abs(firstLong - secondLong) <= LongProximityThreshold
                   && Math.Abs(firstLat - secondLat) <= LatProximityThreshold;
        }

        // algoritme tatt fra
        // http://stackoverflow.com/a/14998816/1770699
        public bool KoordinatErInnenforPolygonet(Koordinat koordinat, Koordinat[] polygon)
        {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].Y < koordinat.Y && polygon[j].Y >= koordinat.Y || polygon[j].Y < koordinat.Y && polygon[i].Y >= koordinat.Y)
                {
                    if (polygon[i].X + (koordinat.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < koordinat.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }
    }
}