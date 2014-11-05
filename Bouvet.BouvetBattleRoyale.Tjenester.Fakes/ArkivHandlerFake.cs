using System.Threading.Tasks;

namespace Bouvet.BouvetBattleRoyale.Tjenester.Fakes
{
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Interfaces;
    
    public class ArkivHandlerFake : IArkivHandler
    {
        private static Task _successTask;

        public ArkivHandlerFake()
        {
            _successTask = Task.FromResult<object>(null);
        }

        public Task SendTilArkivet<T>(T entitet)
        {
            return _successTask;
        }
    }
}