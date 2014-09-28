using System;
using System.Collections.Generic;
using System.Linq;

using BouvetCodeCamp.Domene;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.Domene.InputModels;
using BouvetCodeCamp.Domene.OutputModels;
using BouvetCodeCamp.DomeneTjenester.Interfaces;

namespace BouvetCodeCamp.DomeneTjenester
{
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

        public void RegistrerPifPosition(GeoPosisjonModel model)
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

            _lagService.Oppdater(lag);

            LoggHendelse(model.LagId, HendelseType.RegistrertGeoPosisjon);
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

        /// <summary>
        /// TODO: Denne burde kanskje sorteres.
        /// </summary>
        public IEnumerable<PifPosisjonModel> HentAllePifPosisjoner()
        {
            var alleLag = _lagService.HentAlleLag();
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

        public bool RegistrerKode(KodeModel model)
        {
            var resultat = _kodeService.SettKodeTilstandTilOppdaget(model.LagId, model.Kode, model.Koordinat);

            LoggHendelse(model.LagId, resultat ? HendelseType.RegistrertKodeSuksess : HendelseType.RegistrertKodeMislykket);

            return resultat;
        }

        public void SendMelding(MeldingModel model)
        {
            LoggHendelse(model.LagId, HendelseType.SendtMelding);
        }

        private void LoggHendelse(string lagId, HendelseType hendelseType)
        {
            var lag = _lagService.HentLagMedLagId(lagId);

            lag.LoggHendelser.Add(new LoggHendelse
                                      {
                                          HendelseType = hendelseType, 
                                          Tid = DateTime.Now
                                      });

            _lagService.Oppdater(lag);
        }
    }
}