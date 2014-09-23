using System.Collections.Generic;
using System.Threading.Tasks;
using BouvetCodeCamp.Domene.Entiteter;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    public interface ILagService
    {
        Task<Lag> HentLag(string lagId);
        Task<IEnumerable<Lag>> HentAlleLag();
        void Oppdater(Lag lag);
    }
}
