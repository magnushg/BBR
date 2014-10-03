namespace BouvetCodeCamp.AdminApi
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;

    [RoutePrefix("api/gamestate")]
    [Authorize]
    public class GameStateController : BaseApiController
    {
        private readonly IGameStateService gameStateService;

        public GameStateController(IGameStateService gameStateService)
        {
            this.gameStateService = gameStateService;
        }

        // POST api/gamestate/post
        [Route("post")]
        [HttpPost]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> PostGameState([FromBody]GameState modell)
        {
            if (modell == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig request");

            var gameState = gameStateService.HentGameState();

            gameState.InfisertPolygon = modell.InfisertPolygon;

            await gameStateService.OppdaterGameState(gameState);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}