﻿namespace BouvetCodeCamp.Api.Admin
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using BouvetCodeCamp.Domene;
    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.Domene.OutputModels;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;
    using BouvetCodeCamp.SignalR;

    using Microsoft.AspNet.SignalR;

    /// <summary>
    /// Sett og Hent infiserte soner
    /// </summary>
    [RoutePrefix("api/admin/infisert")]
    [System.Web.Http.Authorize]
    public class InfisertController : BaseApiController
    {
        private readonly IGameStateService _gameStateService;

        private readonly Lazy<IHubContext<IGameHub>> _gameHub;

        public InfisertController(IGameStateService gameStateService,
            Lazy<IHubContext<IGameHub>> gameHub)
        {
            _gameStateService = gameStateService;
            _gameHub = gameHub;
        }

        // GET api/admin/infisert/get
        [HttpGet]
        [Route("get")]
        [Obsolete] // Skjule for Swagger-apidoc
        public HttpResponseMessage GetSone()
        {
            var gameState = _gameStateService.HentGameState();

            try
            {
                var infisertPolygonOutputModell = new InfisertPolygonOutputModell
                {
                    Koordinater = gameState.InfisertPolygon.Koordinater
                };

                return Request.CreateResponse(HttpStatusCode.OK, infisertPolygonOutputModell);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        // POST api/admin/infisert/post
        [HttpPost]
        [Route("post")]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> Post([FromBody] InfisertPolygon modell)
        {
            if (modell == null || modell.Koordinater == null) 
                return OpprettErrorResponse(ErrorResponseType.UgyldigInputFormat, "Modell er ugyldig.");

            var gameState = _gameStateService.HentGameState();
            gameState.InfisertPolygon = modell;

            try
            {
                _gameHub.Value.Clients.All.SettInfisertSone(
                    new InfisertPolygonOutputModell
                    {
                        Koordinater = modell.Koordinater
                    });
                
                await _gameStateService.OppdaterGameState(gameState);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}