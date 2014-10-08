using System.Collections.Generic;
using System.Threading.Tasks;
using BouvetCodeCamp.Domene.Entiteter;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    public interface IPostGameService
    {
        IEnumerable<LagPost> HentOppdagedePoster(string lagId);

        IEnumerable<LagPost> HentAllePosterForLag(string lagId);

        bool SettKodeTilstandTilOppdaget(string lagId, int postnummer, string kode, Koordinat koordinat);
    }
}