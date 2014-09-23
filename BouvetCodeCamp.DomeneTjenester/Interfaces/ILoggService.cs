using BouvetCodeCamp.Domene.Entiteter;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    public interface ILoggService
    {
        void Logg(LoggHendelse hendelse);
    }

    public class FakeLoggSerive : ILoggService
    {
        public void Logg(LoggHendelse hendelse)
        {
            throw new System.NotImplementedException();
        }
    }
}
