namespace Bouvet.BouvetBattleRoyale.Applikasjon.Owin.Api.Admin
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Bouvet.BouvetBattleRoyale.Applikasjon.Owin;
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;

    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;

    [RoutePrefix("api/admin/gamestate")]
    [Authorize]
    public class GameStateController : BaseApiController
    {
        private readonly IService<GameState> gameStateService;

        public GameStateController(IService<GameState> gameStateService)
        {
            this.gameStateService = gameStateService;
        }

        // POST api/admin/gamestate/post
        [Route("post")]
        [HttpPost]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> PostGameState([FromBody]GameState modell)
        {
            if (modell == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ugyldig request");

            var gameState = gameStateService.Hent(string.Empty);

            gameState.InfisertPolygon = modell.InfisertPolygon;

            await gameStateService.Oppdater(gameState);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}