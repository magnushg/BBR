using System.Collections.Generic;
using System.Threading.Tasks;
using BouvetCodeCamp.Domene.Entiteter;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    public interface IPostService
    {
        IEnumerable<LagPost> HentOppdagedePoster(string lagId);
        IEnumerable<LagPost> HentAllePoster(string lagId);
        bool SettKodeTilstandTilOppdaget(string lagId, string kode, Koordinat koordinat);
    }
}
