using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BouvetCodeCamp.Dataaksess.Interfaces;
using BouvetCodeCamp.Felles;
using BouvetCodeCamp.Felles.Entiteter;
using BouvetCodeCamp.InputModels;
using BouvetCodeCamp.OutputModels;

namespace BouvetCodeCamp
{
    public class GameApi:IGameApi
    {
        private readonly IPifPosisjonRepository _pifPosisjonRepository;
        private readonly IAktivitetsloggRepository _aktivitetsloggRepository;

        public GameApi(IPifPosisjonRepository pifPosisjonRepository, IAktivitetsloggRepository aktivitetsloggRepository)
        {
            _pifPosisjonRepository = pifPosisjonRepository;
            _aktivitetsloggRepository = aktivitetsloggRepository;
        }

        public async Task<PifPosisjon> RegistrerPifPosition(GeoPosisjonModel model)
        {
            var pifPosisjon = new PifPosisjon()
            {
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                LagId = model.LagId,
                Tid = DateTime.Now
            };

            await _pifPosisjonRepository.Opprett(pifPosisjon);

            LoggHendelse(string.Empty, HendelseType.RegistrertGeoPosisjon); //TODO hwm 15.09.2014: Noen må sette verdi i LagId

            return pifPosisjon;
        }

        public async Task<PifPosisjonModel> HentSistePifPositionForLag(string lagId)
        {
            var pifPosisjonAll = await _pifPosisjonRepository.HentPifPosisjonerForLag(lagId);
            var pifPosisjon = pifPosisjonAll.FirstOrDefault();

            LoggHendelse(string.Empty, HendelseType.HentetPifPosisjon); //TODO hwm 15.09.2014: Noen må sette verdi i LagId

            return pifPosisjon == null 
                ? null 
                : new PifPosisjonModel
            {
                Latitude = pifPosisjon.Latitude,
                Longitude = pifPosisjon.Longitude,
                LagId = pifPosisjon.LagId
            };
        }

        public async Task<IEnumerable<PifPosisjonModel>> HentAllePifPosisjoner()
        {
            var pifPosisjonAll = await _pifPosisjonRepository.HentAlle();

            LoggHendelse(string.Empty, HendelseType.HentetPifPosisjon); //TODO hwm 15.09.2014: Noen må sette verdi i LagId

            return pifPosisjonAll == null
                ? null
                : pifPosisjonAll.Select(x => new PifPosisjonModel
                {
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    LagId = x.LagId
                });
        }

        public void RegistrerKode(KodeModel model)
        {
            LoggHendelse(string.Empty, HendelseType.RegistrertKode); //TODO hwm 15.09.2014: Noen må sette verdi i LagId
        }

        public void SendMelding(MeldingModel model)
        {
            LoggHendelse(string.Empty, HendelseType.SendtMelding); //TODO hwm 15.09.2014: Noen må sette verdi i LagId
        }

        private async void LoggHendelse(string lagId, HendelseType hendelseType)
        {
            await _aktivitetsloggRepository.Opprett(new AktivitetsloggHendelse
            {
                HendelseType = hendelseType,
                LagId = lagId, 
                Tid = DateTime.Now
            });
        }
    }
}