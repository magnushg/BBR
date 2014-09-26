using System.Collections.Generic;
using System.Threading.Tasks;
using BouvetCodeCamp.Domene.Entiteter;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    public interface ILagService
    {
        Lag HentLag(string lagId);
        IEnumerable<Lag> HentAlleLag();
        void Oppdater(Lag lag);
    }
}
