namespace Bouvet.BouvetBattleRoyale.Tjenester.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Bouvet.BouvetBattleRoyale.Domene;
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Domene.InputModels;
    using Bouvet.BouvetBattleRoyale.Domene.OutputModels;

    using BouvetCodeCamp.Domene.Entiteter;

    public interface IGameApi
    {
        Task RegistrerPifPosisjon(Lag modell, PifPosisjonInputModell inputModell);

        PifPosisjonOutputModell HentSistePifPositionForLag(string lagId);

        Task<bool> RegistrerKode(PostInputModell inputModell);

        Task SendMelding(MeldingInputModell inputModell);

        IEnumerable<KodeOutputModel> HentRegistrerteKoder(string lagId);

        PostOutputModell HentGjeldendePost(string lagId);

        Task TildelPoeng(PoengInputModell inputModell);

        bool ErLagPifInnenInfeksjonssone(string lagId);
        bool ErInfisiert(Koordinat koordinat, GameState gameState);
        IEnumerable<Melding> HentMeldinger(string lagId);

        Task OpprettHendelse(string lagId, HendelseType hendelseType, string kommentar);
    }
}
