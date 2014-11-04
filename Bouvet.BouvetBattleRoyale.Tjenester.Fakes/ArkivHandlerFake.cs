using System.Threading.Tasks;

namespace Bouvet.BouvetBattleRoyale.Tjenester.Fakes
{
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Tjenester.Interfaces;

    public class ArkivHandlerFake : IArkivHandler
    {
        private static Task _successTask;

        public ArkivHandlerFake()
        {
            _successTask = Task.FromResult<object>(null);
        }

        public Task SendTilArkivet(Melding melding)
        {
            return _successTask;
        }
    }
}