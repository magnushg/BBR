using BouvetCodeCamp.Felles.Entiteter;

namespace BouvetCodeCamp.Felles.Interfaces
{
    public interface ICoordinateVerifier
    {
        bool CoordinateSAreInProximity(Coordinate first, Coordinate second);
        bool IsStringValidCoordinate(string coordinate);
        Coordinate ParseCoordinate(string coordinate);
    }
}
