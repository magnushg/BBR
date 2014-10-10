using System;
using System.Linq;

using BouvetCodeCamp.Domene;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.Domene.InputModels;
using BouvetCodeCamp.Domene.OutputModels;
using BouvetCodeCamp.DomeneTjenester.Interfaces;

namespace BouvetCodeCamp.DomeneTjenester
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class GameApi : IGameApi
    {
        private readonly IPostService _postService;
        private readonly ILagService _lagService;
        private readonly IGameStateService _gameStateService;
        private readonly IKoordinatVerifier _koordinatVerifier;
        private readonly IPoengService _poengService;

        public GameApi(
            IPostService postService,
            ILagService lagService,
            IKoordinatVerifier koordinatVerifier,
            IGameStateService gameStateService,
            IPoengService poengService)
        {
            _postService = postService;
            _lagService = lagService;
            _koordinatVerifier = koordinatVerifier;
            _gameStateService = gameStateService;
            _poengService = poengService;
        }

        public async Task RegistrerPifPosisjon(PifPosisjonInputModell inputModell)
        {
            //bemerkning: blir det tungt å hente gamestate for hver pif-ping?
            var gameState = _gameStateService.HentGameState();
            var lag = _lagService.HentLagMedLagId(inputModell.LagId);
            var pifPosisjon = new PifPosisjon
            {
                Posisjon = new Koordinat
                {
                    Latitude = inputModell.Posisjon.Latitude,
                    Longitude = inputModell.Posisjon.Longitude
                },
                LagId = inputModell.LagId,
                Tid = DateTime.Now,
            };

            pifPosisjon.Infisert = ErPifPosisjonInfisert(pifPosisjon, gameState);

            lag.PifPosisjoner.Add(pifPosisjon);

            lag.LoggHendelser.Add(
                new LoggHendelse
                {
                    HendelseType = HendelseType.RegistrertPifPosisjon,
                    Tid = DateTime.Now
                });

            lag = _poengService.SjekkOgSettPifPingStraff(lag);
            lag = _poengService.SjekkOgSettInfisertSoneStraff(lag);

            await _lagService.Oppdater(lag);
        }

        public PifPosisjonOutputModell HentSistePifPositionForLag(string lagId)
        {
            var nyeste = _lagService.HentSistePifPosisjon(lagId);

            if (nyeste == null)
                return new PifPosisjonOutputModell();

            return new PifPosisjonOutputModell
            {
                Latitude = nyeste.Posisjon.Latitude,
                Longitude = nyeste.Posisjon.Longitude,
                LagId = nyeste.LagId,
                Tid = nyeste.Tid,
                Infisert = nyeste.Infisert
            };
        }

        public async Task<bool> RegistrerKode(PostInputModell inputModell)
        {
            var resultat = _postService.SettKodeTilstandTilOppdaget(inputModell.LagId, inputModell.Postnummer, inputModell.Kode, inputModell.Koordinat);

            var lag = _lagService.HentLagMedLagId(inputModell.LagId);

            lag.LoggHendelser.Add(
                new LoggHendelse
                {
                    HendelseType = resultat,
                    Tid = DateTime.Now
                });

            lag = _poengService.SettPoengForKodeRegistrert(lag, resultat);

            await _lagService.Oppdater(lag);

            return resultat.Equals(HendelseType.RegistrertKodeSuksess);
        }

        public async Task SendMelding(MeldingInputModell inputModell)
        {
            var lag = _lagService.HentLagMedLagId(inputModell.LagId);

            var melding = new Melding
            {
                LagId = inputModell.LagId,
                Tekst = inputModell.Tekst,
                Tid = DateTime.Now,
                Type = inputModell.Type
            };

            lag.Meldinger.Add(melding);

            lag.LoggHendelser.Add(
                new LoggHendelse
                {
                    HendelseType = HendelseType.SendtMelding,
                    Tid = DateTime.Now
                });

            lag = _poengService.SettMeldingSendtStraff(lag, melding);

            await _lagService.Oppdater(lag);
        }

        public IEnumerable<KodeOutputModel> HentRegistrerteKoder(string lagId)
        {
            var lag = _lagService.HentLagMedLagId(lagId);

            var registrerteKoderForLag = lag.Poster.Where(o => o.PostTilstand == PostTilstand.Oppdaget);

            return registrerteKoderForLag.Select(registrertKode =>
                new KodeOutputModel
                {
                    Kode = registrertKode.Kode,
                    Koordinat = registrertKode.Posisjon
                }).ToList();
        }

        public PostOutputModell HentGjeldendePost(string lagId)
        {
            var lag = _lagService.HentLagMedLagId(lagId);

            return
                OpprettPostOutput(
                    lag.Poster.OrderBy(post => post.Sekvensnummer)
                        .First(post => post.PostTilstand == PostTilstand.Ukjent));


        }

        [Obsolete]
        public async Task TildelPoeng(PoengInputModell inputModell)
        {
            var lag = _lagService.HentLagMedLagId(inputModell.LagId);

            lag.Poeng += inputModell.Poeng;

            lag.LoggHendelser.Add(
                new LoggHendelse
                {
                    HendelseType = HendelseType.TildeltPoeng,
                    Tid = DateTime.Now
                });

            await _lagService.Oppdater(lag);
        }

        public bool ErLagPifInnenInfeksjonssone(string lagId)
        {
            var pifPosisjon = _lagService.HentSistePifPosisjon(lagId);
            var gameState = _gameStateService.HentGameState();

            return ErPifPosisjonInfisert(pifPosisjon, gameState);
        }

        [Obsolete("PifPosisjon har parameter infisert allerede")]
        private bool ErPifPosisjonInfisert(PifPosisjon posisjon, GameState gameState)
        {
            return _koordinatVerifier.KoordinatErInnenforPolygonet(posisjon.Posisjon, gameState.InfisertPolygon.Koordinater);
        }

        public IEnumerable<Melding> HentMeldinger(string lagId)
        {
            var lag = _lagService.HentLagMedLagId(lagId);

            return lag.Meldinger;
        }

        private PostOutputModell OpprettPostOutput(LagPost post)
        {
            return new PostOutputModell
            {
                Navn = post.Navn,
                Nummer = post.Nummer,
                Posisjon = post.Posisjon
            };
        }
    }
}