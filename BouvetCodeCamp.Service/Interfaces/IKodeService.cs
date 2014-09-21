using System.Collections.Generic;
using System.Threading.Tasks;
using BouvetCodeCamp.Felles.Entiteter;

namespace BouvetCodeCamp.Service.Interfaces
{
    public interface IKodeService
    {
        Task<IEnumerable<Kode>> HentOppdagedeKoder(string lagId);
        Task<IEnumerable<Kode>> HentAlleKoder(string lagId);
        Task<bool> SettKodeTilstandTilOppdaget(string lagId, string kode, Coordinate koordinat);
    }
}
