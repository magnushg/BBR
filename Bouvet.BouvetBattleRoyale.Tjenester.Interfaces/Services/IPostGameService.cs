namespace Bouvet.BouvetBattleRoyale.Tjenester.Interfaces.Services
{
    using System.Collections.Generic;

    using Bouvet.BouvetBattleRoyale.Domene;
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;

    public interface IPostGameService
    {
        IEnumerable<LagPost> HentOppdagedePoster(Lag lag);

        IEnumerable<LagPost> HentAllePosterForLag(Lag lag);

        HendelseType SettKodeTilstandTilOppdaget(Lag lag, int postnummer, string kode, Koordinat koordinat);
    }
}