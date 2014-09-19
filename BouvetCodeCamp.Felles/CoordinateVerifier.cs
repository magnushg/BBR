using System;
using BouvetCodeCamp.Felles.Entiteter;
using BouvetCodeCamp.Felles.Interfaces;

namespace BouvetCodeCamp.Felles
{
    public class CoordinateVerifier : ICoordinateVerifier
    {
        private const int ProximityThreshold = 10;

        public bool CoordinateSAreInProximity(Coordinate first, Coordinate second)
        {
            return true;
        }

        public string IsStringValidCoordinate(string coordinate)
        {
            throw new NotImplementedException();
        }
    }
}
