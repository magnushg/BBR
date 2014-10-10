using System.Collections.Generic;
using BouvetCodeCamp.Domene;
using BouvetCodeCamp.Domene.Entiteter;

namespace BouvetCodeCamp.DomeneTjenester.Interfaces
{
    public interface IPostGameService
    {
        IEnumerable<LagPost> HentOppdagedePoster(Lag lag);

        IEnumerable<LagPost> HentAllePosterForLag(Lag lag);

        HendelseType SettKodeTilstandTilOppdaget(Lag lag, int postnummer, string kode, Koordinat koordinat);
    }
}