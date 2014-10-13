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
        private readonly IPostGameService _postGameService;
        private readonly ILagGameService _lagGameService;
        private readonly IService<Lag> _lagService;
        private readonly IService<GameState> _gameStateService;
        private readonly IKoordinatVerifier _koordinatVerifier;

        public GameApi(
            IPostGameService postGameService,
            ILagGameService lagGameService,
            IService<Lag> lagService,
            IKoordinatVerifier koordinatVerifier,
            IService<GameState> gameStateService)
        {
            _postGameService = postGameService;
            _lagGameService = lagGameService;
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

            var lag = _lagGameService.HentLagMedLagId(inputModell.LagId);
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
            var nyeste = _lagGameService.HentSistePifPosisjon(lagId);

            if (nyeste == null)
                return new PifPosisjonOutputModell();
            var erInfisert = false;
            try
            {
                erInfisert = ErLagPifInnenInfeksjonssone(lagId);
            }
            catch (Exception)
            {                
            }
            return new PifPosisjonOutputModell
            {
                Latitude = nyeste.Posisjon.Latitude,
                Longitude = nyeste.Posisjon.Longitude,
                LagId = nyeste.LagId,
                Tid = nyeste.Tid,
                ErInfisert = erInfisert
            };
        }

        public async Task<bool> RegistrerKode(PostInputModell inputModell)
        {
            //TODO: Tildel poeng via egen modul. Returnerer lag fra modulen. Input er hendelse.
            
            var lag = _lagGameService.HentLagMedLagId(inputModell.LagId);

            var resultat = _postGameService.SettKodeTilstandTilOppdaget(lag, inputModell.Postnummer, inputModell.Kode, inputModell.Koordinat);
            
            lag.LoggHendelser.Add(
                new LoggHendelse
                {
                    HendelseType = resultat ? HendelseType.RegistrertKodeSuksess : HendelseType.RegistrertKodeMislykket,
                    Tid = DateTime.Now
                });

            await _lagService.Oppdater(lag);

            return resultat;
        }

        public async Task SendMelding(MeldingInputModell inputModell)
        {
            var lag = _lagGameService.HentLagMedLagId(inputModell.LagId);

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
            var lag = _lagGameService.HentLagMedLagId(lagId);

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
            var lag = _lagGameService.HentLagMedLagId(lagId);

            return
                OpprettPostOutput(lag.Poster
                        .OrderBy(post => post.Sekvensnummer)
                        .First(post => post.PostTilstand == PostTilstand.Ukjent));
        }

        public async Task TildelPoeng(PoengInputModell inputModell)
        {
            var lag = _lagGameService.HentLagMedLagId(inputModell.LagId);

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
            var pifPosisjon = _lagGameService.HentSistePifPosisjon(lagId);

            if (pifPosisjon == null) 
                return false;


            return ErInfisiert(pifPosisjon.Posisjon);

        }

        public bool ErInfisiert(Koordinat koordinat)
        {
            var gameState = _gameStateService.Hent(string.Empty);
            return  _koordinatVerifier.KoordinatErInnenforPolygonet(koordinat, gameState.InfisertPolygon.Koordinater);
        }

        public IEnumerable<Melding> HentMeldinger(string lagId)
        {
            var lag = _lagGameService.HentLagMedLagId(lagId);

            return lag.Meldinger;
        }

        public async Task OpprettHendelse(string lagId, HendelseType hendelseType, string kommentar)
        {
            var lag = _lagGameService.HentLagMedLagId(lagId);

            lag.LoggHendelser.Add(new LoggHendelse
            {
                HendelseType = hendelseType,
                Kommentar = kommentar,
                Tid = DateTime.Now
            });

            await _lagService.Oppdater(lag);
        }

        private PostOutputModell OpprettPostOutput(LagPost post)
        {
            if (post == null) 
                return null;

            return new PostOutputModell
            {
                Navn = post.Navn,
                Nummer = post.Nummer,
                Posisjon = post.Posisjon
            };
        }
    }
}