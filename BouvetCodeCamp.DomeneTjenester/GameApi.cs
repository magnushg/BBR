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

        public GameApi(
            IPostService postService,
            ILagService lagService,
            IKoordinatVerifier koordinatVerifier,
            IGameStateService gameStateService)
        {
            _postService = postService;
            _lagService = lagService;
            _koordinatVerifier = koordinatVerifier;
            _gameStateService = gameStateService;
        }

        public async Task RegistrerPifPosisjon(PifPosisjonInputModell inputModell)
        {
            var pifPosisjon = new PifPosisjon
            {
                Posisjon = new Koordinat
                {
                    Latitude = inputModell.Posisjon.Latitude,
                    Longitude = inputModell.Posisjon.Longitude
                },
                LagId = inputModell.LagId,
                Tid = DateTime.Now
            };

            var lag = _lagService.HentLagMedLagId(inputModell.LagId);
            lag.PifPosisjoner.Add(pifPosisjon);

            lag.LoggHendelser.Add(
                new LoggHendelse
                {
                    HendelseType = HendelseType.RegistrertPifPosisjon,
                    Tid = DateTime.Now
                });

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
                Tid = nyeste.Tid
            };
        }

        public async Task<bool> RegistrerKode(PostInputModell inputModell)
        {
            var resultat = _postService.SettKodeTilstandTilOppdaget(inputModell.LagId, inputModell.Postnummer, inputModell.Kode, inputModell.Koordinat);

            await LoggHendelse(inputModell.LagId, resultat ? HendelseType.RegistrertKodeSuksess : HendelseType.RegistrertKodeMislykket);

            return resultat;
        }

        public async Task SendMelding(MeldingInputModell inputModell)
        {
            var lag = _lagService.HentLagMedLagId(inputModell.LagId);

            lag.Meldinger.Add(
                new Melding
                {
                    LagId = inputModell.LagId,
                    Tekst = inputModell.Tekst,
                    Tid = DateTime.Now,
                    Type = inputModell.Type
                });

            lag.LoggHendelser.Add(
                new LoggHendelse
                {
                    HendelseType = HendelseType.SendtMelding,
                    Tid = DateTime.Now
                });

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

            return _koordinatVerifier.KoordinatErInnenforPolygonet(pifPosisjon.Posisjon, gameState.InfisertPolygon.Koordinater);
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

        private async Task LoggHendelse(string lagId, HendelseType hendelseType)
        {
            var lag = _lagService.HentLagMedLagId(lagId);

            lag.LoggHendelser.Add(new LoggHendelse
            {
                HendelseType = hendelseType,
                Tid = DateTime.Now
            });

            await _lagService.Oppdater(lag);
        }
    }
}