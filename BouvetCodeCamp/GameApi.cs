using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BouvetCodeCamp.Felles;
using BouvetCodeCamp.Felles.Entiteter;
using BouvetCodeCamp.InputModels;
using BouvetCodeCamp.OutputModels;
using BouvetCodeCamp.Service.Interfaces;

namespace BouvetCodeCamp
{
    public class GameApi : IGameApi
    {
        private readonly IKodeService _kodeService;
        private readonly ILagService _lagService;
        private readonly ILoggService loggService;

        public GameApi(
            IKodeService kodeService,
            ILagService lagService,
            ILoggService loggService)
        {
            _kodeService = kodeService;
            _lagService = lagService;
            this.loggService = loggService;
        }

        public async Task<PifPosisjon> RegistrerPifPosition(GeoPosisjonModel model)
        {
            var pifPosisjon = new PifPosisjon
            {
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                LagId = model.LagId,
                Tid = DateTime.Now
            };

            var lag = await _lagService.HentLag(model.LagId);
            lag.PifPosisjoner.Add(pifPosisjon);

            _lagService.Oppdater(lag);

            LoggHendelse(model.LagId, HendelseType.RegistrertGeoPosisjon);

            return pifPosisjon;
        }

        public async Task<PifPosisjonModel> HentSistePifPositionForLag(string lagId)
        {
            var lag = await _lagService.HentLag(lagId);

            var sortertListe = lag.PifPosisjoner.OrderBy(x => x.Tid);
            var nyeste = sortertListe.FirstOrDefault();

            if (nyeste == null) return null;

            return new PifPosisjonModel
            {
                Latitude = nyeste.Latitude,
                Longitude = nyeste.Longitude,
                LagId = nyeste.LagId,
                Tid = nyeste.Tid
            };
        }

        /// <summary>
        /// TODO: Denne burde kanskje sorteres.
        /// </summary>
        public async Task<IEnumerable<PifPosisjonModel>> HentAllePifPosisjoner()
        {
            var alleLag = await _lagService.HentAlleLag();
            var posisjoner = alleLag.SelectMany(x => x.PifPosisjoner);

            return posisjoner.Select(x =>
                new PifPosisjonModel
                {
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    LagId = x.LagId,
                    Tid = x.Tid
                }
            );
        }

        public async Task<bool> RegistrerKode(KodeModel model)
        {
            var resultat = await _kodeService.SettKodeTilstandTilOppdaget(model.LagId, model.Kode, model.Koordinat);

            LoggHendelse(model.LagId, resultat ? HendelseType.RegistrertKodeSuksess : HendelseType.RegistrertKodeMislykket);

            return resultat;
        }

        public void SendMelding(MeldingModel model)
        {
            LoggHendelse(model.LagId, HendelseType.SendtMelding);
        }

        private void LoggHendelse(string lagId, HendelseType hendelseType)
        {
            this.loggService.Logg(new LoggHendelse
            {
                HendelseType = hendelseType,
                LagId = lagId,
                Tid = DateTime.Now
            });
        }
    }
}