using System.Collections.Generic;
using System.Threading.Tasks;
using BouvetCodeCamp.Domene.Entiteter;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    public interface IKodeService
    {
        IEnumerable<Kode> HentOppdagedeKoder(string lagId);
        IEnumerable<Kode> HentAlleKoder(string lagId);
        bool SettKodeTilstandTilOppdaget(string lagId, string kode, Coordinate koordinat);
    }
}
