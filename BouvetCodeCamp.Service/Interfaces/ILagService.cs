using System.Collections.Generic;
using System.Threading.Tasks;
using BouvetCodeCamp.Felles.Entiteter;

namespace BouvetCodeCamp.Service.Interfaces
{
    public interface ILagService
    {
        Task<Lag> HentLag(string lagId);
        Task<IEnumerable<Lag>> HentAlleLag();
        void Oppdater(Lag lag);
    }
}
