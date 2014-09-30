using System.Web.Http;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester.Interfaces;

namespace BouvetCodeCamp.AdminApi
{
    using System;

    [RoutePrefix("api/infisert")]
    [Authorize]
    public class InfisertController : ApiController
    {
        private readonly IGameStateService _gameStateService;

        public InfisertController(IGameStateService gameStateService)
        {
            _gameStateService = gameStateService;
        }

        // GET api/infisert/get
        [HttpGet]
        [Obsolete]
        public InfisertPolygon Get()
        {
            var gameState = _gameStateService.HentGameState();
            return gameState.InfisertPolygon;
        }

        // POST api/infisert/post
        [HttpPost]
        [Obsolete]
        public void Post([FromBody] InfisertPolygon model)
        {
            var gameState = _gameStateService.HentGameState();
            gameState.InfisertPolygon = model;

            _gameStateService.OppdaterGameState(gameState);
        }
    }
}