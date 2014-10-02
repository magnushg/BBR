using System.Web.Http;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester.Interfaces;

namespace BouvetCodeCamp.AdminApi
{
    using System;

    using BouvetCodeCamp.Domene.InputModels;

    [RoutePrefix("api/infisert")]
    [Authorize]
    public class InfisertController : BaseApiController
    {
        private readonly IGameStateService _gameStateService;

        public InfisertController(IGameStateService gameStateService)
        {
            _gameStateService = gameStateService;
        }

        // GET api/infisert/get
        [Route("get")]
        [HttpGet]
        [Obsolete] // Skjule for Swagger-apidoc
        public InfisertPolygon Get()
        {
            var gameState = _gameStateService.HentGameState();
            return gameState.InfisertPolygon;
        }

        // POST api/infisert/post
        [Route("post")]
        [HttpPost]
        [Obsolete] // Skjule for Swagger-apidoc
        public void Post([FromBody] InfisertPolygonInputModell modell)
        {
            var gameState = _gameStateService.HentGameState();

            gameState.InfisertPolygon = new InfisertPolygon {
                                                Koordinats = modell.Koordinater
                                            };

            _gameStateService.OppdaterGameState(gameState);
        }
    }
}