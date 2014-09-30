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

        public GameApi(
            IPostService postService,
            ILagService lagService)
        {
            _postService = postService;
            _lagService = lagService;
        }
        
        public async Task RegistrerPifPosisjon(PifPosisjonModell modell)
        {
            var pifPosisjon = new PifPosisjon
            {
                Posisjon = new Koordinat
                {
                    Latitude = modell.Latitude,
                    Longitude = modell.Longitude
                },
                LagId = modell.LagId,
                Tid = DateTime.Now
            };

            var lag = _lagService.HentLagMedLagId(modell.LagId);
            lag.PifPosisjoner.Add(pifPosisjon);

            lag.LoggHendelser.Add(
                new LoggHendelse
                {
                    HendelseType = HendelseType.RegistrertGeoPosisjon,
                    Tid = DateTime.Now
                });

            await _lagService.Oppdater(lag);
        }

        public PifPosisjonModel HentSistePifPositionForLag(string lagId)
        {
            var lag = _lagService.HentLagMedLagId(lagId);

            var sortertListe = lag.PifPosisjoner.OrderBy(x => x.Tid);
            var nyeste = sortertListe.FirstOrDefault();

            if (nyeste == null)
                return new PifPosisjonModel();

            return new PifPosisjonModel
            {
                Latitude = nyeste.Posisjon.Latitude,
                Longitude = nyeste.Posisjon.Longitude,
                LagId = nyeste.LagId,
                Tid = nyeste.Tid
            };
        }

        public async Task<bool> RegistrerKode(KodeModel model)
        {
            var resultat = _postService.SettKodeTilstandTilOppdaget(model.LagId, model.Kode, model.Koordinat);

            await LoggHendelse(model.LagId, resultat ? HendelseType.RegistrertKodeSuksess : HendelseType.RegistrertKodeMislykket);

            return resultat;
        }

        public async Task SendMelding(MeldingModell modell)
        {
            var lag = _lagService.HentLagMedLagId(modell.LagId);

            lag.Meldinger.Add(
                new Melding
                {
                    LagId = modell.LagId,
                    Tekst = modell.Tekst,
                    Tid = DateTime.Now,
                    Type = modell.Type
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