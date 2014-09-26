namespace BouvetCodeCamp.GameApi
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using BouvetCodeCamp.Domene;
    using BouvetCodeCamp.Domene.InputModels;
    using BouvetCodeCamp.Domene.OutputModels;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;
    using BouvetCodeCamp.SignalR;

    using Microsoft.AspNet.SignalR;

    [RoutePrefix("api/game/pif")]
    public class PifGameController : ApiController
    {
        private readonly IGameApi _gameApi;
        Lazy<IHubContext<IGameHub>> _gameHub;

        public PifGameController(IGameApi gameApi)
        {
            this._gameApi = gameApi;
        }

        // POST api/game/pif/SendPifPosition
        [HttpPost]
        [Route("sendPifPosition")]
        public HttpResponseMessage SendPifPosition([FromUri] GeoPosisjonModel modell)
        {
            if (modell == null) 
                return this.OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat);

            //Øverst til venstre 59.680782, 10.602574
            //Nederst til høyre 59.672267, 10.609526

            this._gameHub.Value.Clients.All.NyPifPosisjon(new PifPosisjonModel { LagId = modell.LagId, Latitude = modell.Latitude, Longitude = modell.Longitude, Tid = DateTime.Now });
            var nyPosisjon = this._gameApi.RegistrerPifPosition(modell);
           
            return this.Request.CreateResponse(HttpStatusCode.OK, nyPosisjon);
        }
        
        // POST api/game/pif/sendpostkode
        [HttpPost]
        [Route("sendpostkode")]
        public HttpResponseMessage SendPostKode([FromUri] KodeModel modell)
        {
            if (modell == null)
                return this.OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat);

            try
            {
                var kodeRegistrert = this._gameApi.RegistrerKode(modell);

                return kodeRegistrert ?
                    this.Request.CreateResponse(HttpStatusCode.OK) :
                    this.Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        // GET api/game/pif/erinfisert
        [HttpGet]
        [Route("erinfisert")]
        public void ErInfisert()
        {
           
        }
        
        private HttpResponseMessage OpprettErrorResponse(ErrorResponseType errorResponseType)
        {
            switch (errorResponseType)
            {
                case ErrorResponseType.UgyldigInputFormat:
                    return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig inputformat");
            }
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig forespørsel");
        }
    }
}