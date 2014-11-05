namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Interfaces
{
    using System.Threading.Tasks;

    public interface IArkivHandler
    {
        Task SendTilArkivet<T>(T entitet);
    }
}