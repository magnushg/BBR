using System.Threading.Tasks;
using BouvetCodeCamp.Felles.Entiteter;

namespace BouvetCodeCamp.Service.Interfaces
{
    public interface ILagService
    {
        Task<Lag> HentLag(string lagId);
        void Oppdater(Lag lag);
    }
}
