using BouvetCodeCamp.Felles.Entiteter;

namespace BouvetCodeCamp.Felles.Interfaces
{
    public interface ICoordinateVerifier
    {
        bool CoordinateSAreInProximity(Coordinate first, Coordinate second);
        string IsStringValidCoordinate(string coordinate);
    }
}
