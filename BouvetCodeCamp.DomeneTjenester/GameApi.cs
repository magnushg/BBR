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

    using PifPosisjonModell = BouvetCodeCamp.Domene.OutputModels.PifPosisjonModell;

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
        
        public async Task RegistrerPifPosisjon(Domene.InputModels.PifPosisjonModell modell)
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
                    HendelseType = HendelseType.RegistrertPifPosisjon,
                    Tid = DateTime.Now
                });

            await _lagService.Oppdater(lag);
        }

        public PifPosisjonModell HentSistePifPositionForLag(string lagId)
        {
            var lag = _lagService.HentLagMedLagId(lagId);

            var sortertListe = lag.PifPosisjoner.OrderBy(x => x.Tid);
            var nyeste = sortertListe.FirstOrDefault();

            if (nyeste == null)
                return new PifPosisjonModell();

            return new PifPosisjonModell
            {
                Latitude = nyeste.Posisjon.Latitude,
                Longitude = nyeste.Posisjon.Longitude,
                LagId = nyeste.LagId,
                Tid = nyeste.Tid
            };
        }

        public async Task<bool> RegistrerKode(KodeModell modell)
        {
            var resultat = _postService.SettKodeTilstandTilOppdaget(modell.LagId, modell.Kode, modell.Koordinat);

            await LoggHendelse(modell.LagId, resultat ? HendelseType.RegistrertKodeSuksess : HendelseType.RegistrertKodeMislykket);

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