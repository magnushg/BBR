using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester.Interfaces;

namespace BouvetCodeCamp.DomeneTjenester
{
    public class CoordinateVerifier : ICoordinateVerifier
    {
        public static double LongProximityThreshold = 10;
        public static double LatProximityThreshold = 10;

        //http://stackoverflow.com/a/18690202/1770699
        private readonly Regex _match = new Regex(@"^[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)$");

        public bool CoordinatesAreInProximity(Coordinate first, Coordinate second)
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
        public bool CoordinateIsInPolygon(Coordinate point, Coordinate[] polygon)
        {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].Y < point.Y && polygon[j].Y >= point.Y || polygon[j].Y < point.Y && polygon[i].Y >= point.Y)
                {
                    if (polygon[i].X + (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < point.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        public bool IsStringValidCoordinate(string coordinate)
        {
            return _match.IsMatch(coordinate);
        }

        public Coordinate ParseCoordinate(string coordinate)
        {
            if (!IsStringValidCoordinate(coordinate))
                throw new ArgumentException("ugyldig koordinat");

            var coordinates = coordinate.Trim().Split(',');

            return new Coordinate(coordinates[0], coordinates[1]);
        }
    }
}
