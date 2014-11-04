namespace Bouvet.BouvetBattleRoyale.Tjenester.Interfaces
{
    using System.Threading.Tasks;

    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;

    public interface IArkivHandler
    {
        Task SendTilArkivet(Melding melding);
    }
}