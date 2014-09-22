using System;
using System.Text.RegularExpressions;
using BouvetCodeCamp.Felles.Entiteter;
using BouvetCodeCamp.Felles.Interfaces;

namespace BouvetCodeCamp.Felles
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
