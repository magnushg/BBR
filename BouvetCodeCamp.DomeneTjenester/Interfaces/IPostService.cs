using System.Collections.Generic;
using System.Threading.Tasks;
using BouvetCodeCamp.Domene;
using BouvetCodeCamp.Domene.Entiteter;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    public interface IPostService
    {
        IEnumerable<LagPost> HentOppdagedePoster(string lagId);
        IEnumerable<LagPost> HentAllePosterForLag(string lagId);
        HendelseType SettKodeTilstandTilOppdaget(string lagId, int postnummer, string kode, Koordinat koordinat);

        IEnumerable<Post> HentAlle();

        Post Hent(string id);

        Task Opprett(Post post);

        Task Oppdater(Post post);

        Task SlettAlle();

        Task Slett(Post post);
    }
}
