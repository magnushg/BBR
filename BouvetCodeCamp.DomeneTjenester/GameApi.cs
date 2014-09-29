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
        private readonly IKodeService _kodeService;
        private readonly ILagService _lagService;

        public GameApi(
            IKodeService kodeService,
            ILagService lagService)
        {
            _kodeService = kodeService;
            _lagService = lagService;
        }

        public async void RegistrerPifPosition(GeoPosisjonModel model)
        {
            var pifPosisjon = new PifPosisjon
            {
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                LagId = model.LagId,
                Tid = DateTime.Now
            };

            var lag = _lagService.HentLagMedLagId(model.LagId);
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
                Latitude = nyeste.Latitude,
                Longitude = nyeste.Longitude,
                LagId = nyeste.LagId,
                Tid = nyeste.Tid
            };
        }

        public async Task<bool> RegistrerKode(KodeModel model)
        {
            var resultat = _kodeService.SettKodeTilstandTilOppdaget(model.LagId, model.Kode, model.Koordinat);

            await LoggHendelse(model.LagId, resultat ? HendelseType.RegistrertKodeSuksess : HendelseType.RegistrertKodeMislykket);

            return resultat;
        }

        public async Task SendMelding(MeldingModel model)
        {
            var lag = _lagService.HentLagMedLagId(model.LagId);

            lag.Meldinger.Add(
                new Melding
                    {
                        LagId = model.LagId, 
                        Tekst = model.Tekst, 
                        Tid = DateTime.Now, 
                        Type = model.Type
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

            var registrerteKoderForLag = lag.Koder.Where(o => o.PosisjonTilstand == PosisjonTilstand.Oppdaget);

            return registrerteKoderForLag.Select(registrertKode => 
                new KodeOutputModel
                    {
                        Kode = registrertKode.Bokstav, 
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