using System;
using System.Linq;

using BouvetCodeCamp.Domene;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.Domene.InputModels;
using BouvetCodeCamp.Domene.OutputModels;
using BouvetCodeCamp.DomeneTjenester.Interfaces;
using BouvetCodeCamp.SignalR.Hubs;
using Microsoft.AspNet.SignalR;

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
        private readonly IPoengService _poengService;
        private readonly IGameHub _gameHub;

        public GameApi(
            IPostGameService postGameService,
            ILagGameService lagGameService,
            IService<Lag> lagService,
            IKoordinatVerifier koordinatVerifier,
            IService<GameState> gameStateService,
            IPoengService poengService,
            IGameHub gameHub)
        {
            _postGameService = postGameService;
            _lagGameService = lagGameService;
            _lagService = lagService;
            _koordinatVerifier = koordinatVerifier;
            _gameStateService = gameStateService;
            _poengService = poengService;
            _gameHub = gameHub;
        }

        public async Task RegistrerPifPosisjon(Lag lag, PifPosisjonInputModell inputModell)
        {
            //bemerkning: blir det tungt å hente gamestate for hver pif-ping?
            var gameState = _gameStateService.Hent(String.Empty);

            var koordinat = new Koordinat
                {
                    Latitude = inputModell.Posisjon.Latitude,
                    Longitude = inputModell.Posisjon.Longitude
                };

            var pifPosisjon = new PifPosisjon
            {
                Posisjon = koordinat,
                LagId = inputModell.LagId,
                Tid = DateTime.Now,
                Infisert = ErInfisiert(koordinat, gameState)
            };

            lag.PifPosisjoner.Add(pifPosisjon);

            SørgForIkkeForMangePifPosisjoner(lag);

            lag = _poengService.SjekkOgSettInfisertSoneStraff(lag);

            _gameHub.NyPifPosisjon(
                    new PifPosisjonOutputModell
                    {
                        LagId = inputModell.LagId,
                        Latitude = inputModell.Posisjon.Latitude,
                        Longitude = inputModell.Posisjon.Longitude,
                        Tid = DateTime.Now,
                        ErInfisert = pifPosisjon.Infisert
                    });

            await _lagService.Oppdater(lag);
        }

        private static void SørgForIkkeForMangePifPosisjoner(Lag lag)
        {
            if (lag.PifPosisjoner.Count > 5)
            {
                lag.PifPosisjoner.Remove(lag.PifPosisjoner.First());
            }
        }

        public PifPosisjonOutputModell HentSistePifPositionForLag(string lagId)
        {
            var nyeste = _lagGameService.HentSistePifPosisjon(lagId);

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
            var lag = _lagGameService.HentLagMedLagId(inputModell.LagId);

            var resultat = _postGameService.SettKodeTilstandTilOppdaget(lag, inputModell.Postnummer, inputModell.Kode, inputModell.Koordinat);

            lag = _poengService.SettPoengForKodeRegistrert(lag, resultat, inputModell.Postnummer);

            await _lagService.Oppdater(lag);

            if (resultat.Equals(HendelseType.RegistrertKodeSuksess))
            {
                SendPostRegistrertHendelse(lag.LagId, inputModell.Postnummer);

                return true;
            }
            return false;
        }

        public async Task SendMelding(MeldingInputModell inputModell)
        {
            MeldingInputModelIsValid(inputModell);

            var lag = _lagGameService.HentLagMedLagId(inputModell.LagId);

            var melding = new Melding
            {
                LagId = inputModell.LagId,
                Tekst = inputModell.Innhold,
                Tid = DateTime.Now,
                Type = inputModell.Type
            };

            lag.Meldinger.Add(melding);

            lag = _poengService.SettFritekstMeldingSendtStraff(lag, melding);

            await _lagService.Oppdater(lag);
        }

        private void MeldingInputModelIsValid(MeldingInputModell model)
        {
            switch (model.Type)
            {
                case (MeldingType.Fritekst):

                    if (model.Innhold.Length > 256)
                        throw new MeldingException("Fritekst er over 256 karakterer");
                    break;

                case (MeldingType.Himmelretning):

                    Himmelretning cast;

                    if (!Enum.IsDefined(typeof(Himmelretning), model.Innhold))
                    {
                        throw new MeldingException(
                            model.Innhold + " er ikke gyldig himmelretning-verdi (husk den er case-sensitive)");
                    }
                    break;

                case (MeldingType.Lengde):
                    try
                    {
                        int.Parse(model.Innhold);
                    }
                    catch (Exception)
                    {
                        throw new MeldingException("Meldingstype Lengde må være integer");
                    }
                    break;

                case (MeldingType.Stopp):
                    var boolskVerdi = model.Innhold.ToLower();
                    if (!(boolskVerdi.Equals("true") || boolskVerdi.Equals("false")))
                    {
                        throw new MeldingException("Meldingstype Stopp må være boolsk verdi");
                    }
                    break;
            }
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

            return OpprettPostOutput(lag.Poster
                        .OrderBy(post => post.Sekvensnummer)
                        .FirstOrDefault(post => post.PostTilstand == PostTilstand.Ukjent));
        }

        public async Task TildelPoeng(PoengInputModell inputModell)
        {
            var lag = _lagGameService.HentLagMedLagId(inputModell.LagId);

            var lagMedPoeng = _poengService.SettPoengForLag(lag, inputModell.Poeng, inputModell.Kommentar);

            await _lagService.Oppdater(lagMedPoeng);
        }

        public bool ErLagPifInnenInfeksjonssone(string lagId)
        {
            var pifPosisjon = _lagGameService.HentSistePifPosisjon(lagId);

            if (pifPosisjon == null)
                return false;

            return pifPosisjon.Infisert;
        }

        public bool ErInfisiert(Koordinat koordinat, GameState gameState)
        {
            return _koordinatVerifier.KoordinatErInnenforPolygonet(koordinat, gameState.InfisertPolygon.Koordinater);
        }

        public IEnumerable<Melding> HentMeldinger(string lagId)
        {
            var lag = _lagGameService.HentLagMedLagId(lagId);

            return lag.Meldinger;
        }

        public async Task OpprettHendelse(string lagId, HendelseType hendelseType, string kommentar)
        {
            var lag = _lagGameService.HentLagMedLagId(lagId);
            
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

        private void SendPostRegistrertHendelse(string lagId, int postnummer)
        {
            _gameHub.NyPostRegistrert(new PostRegistrertOutputModell
            {
                LagId = lagId,
                Nummer = postnummer
            });
        }
    }
}