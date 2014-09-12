using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BouvetCodeCamp.Dataaksess.Interfaces;
using BouvetCodeCamp.Felles.Entiteter;
using BouvetCodeCamp.InputModels;
using BouvetCodeCamp.OutputModels;

namespace BouvetCodeCamp
{
    public class GameApi:IGameApi
    {
        private readonly IPifPosisjonRepository _pifPosisjonRepository;

        public GameApi(IPifPosisjonRepository pifPosisjonRepository)
        {
            _pifPosisjonRepository = pifPosisjonRepository;
        }

        public HttpResponseMessage RegistrerPifPosition(GeoPosisjonModel model)
        {
            var pifPosisjon = new PifPosisjon()
            {
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                LagId = int.Parse(model.LagId),
                Tid = DateTime.Now
            };
            _pifPosisjonRepository.Opprett(pifPosisjon);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        public async Task<PifPosisjonModel> GetPifPosition(string lagId)
        {
            var pifPosisjonAll = await _pifPosisjonRepository.HentPifPosisjon(lagId);
            var pifPosisjon = pifPosisjonAll.FirstOrDefault();

            return pifPosisjon == null 
                ? null 
                : new PifPosisjonModel
            {
                Latitude = pifPosisjon.Latitude,
                Longitude = pifPosisjon.Longitude,
                LagId = ""+pifPosisjon.LagId
            };
        }

        public async Task<IEnumerable<PifPosisjonModel>> GetAllPifPositions()
        {
            var pifPosisjonAll = await _pifPosisjonRepository.HentAlle();

            return pifPosisjonAll == null
                ? null
                : pifPosisjonAll.Select(x => new PifPosisjonModel
                {
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    LagId = ""+x.LagId
                });
        }

        public HttpResponseMessage RegistrerKode(KodeModel model)
        {
            throw new NotImplementedException();
        }

        public HttpResponseMessage SendMelding(MeldingModel model)
        {
            throw new NotImplementedException();
        }

        public HttpResponseMessage HentPifPosisjon(string lagId)
        {
            throw new NotImplementedException();
        }
    }
}
