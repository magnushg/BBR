using BouvetCodeCamp.Domene.Entiteter;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    public interface ICoordinateVerifier
    {
        bool CoordinatesAreInProximity(Coordinate first, Coordinate second);
        bool IsStringValidCoordinate(string coordinate);
        Coordinate ParseCoordinate(string coordinate);
    }
}
