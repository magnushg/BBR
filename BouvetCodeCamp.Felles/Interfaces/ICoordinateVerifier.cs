using BouvetCodeCamp.Felles.Entiteter;

namespace BouvetCodeCamp.Felles.Interfaces
{
    public interface ICoordinateVerifier
    {
        bool CoordinatesAreInProximity(Coordinate first, Coordinate second);
        bool IsStringValidCoordinate(string coordinate);
        Coordinate ParseCoordinate(string coordinate);
    }
}
