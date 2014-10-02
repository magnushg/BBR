using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester.Interfaces;

namespace BouvetCodeCamp.AdminApi
{
    using System;

    /// <summary>
    /// Sett og Hent infiserte soner
    /// </summary>
    [RoutePrefix("api/admin/infisert")]
//    [Authorize]
    public class InfisertController : ApiController
    {
        private readonly IGameStateService _gameStateService;

        public InfisertController(IGameStateService gameStateService)
        {
            _gameStateService = gameStateService;
        }

        // GET api/infisert
        /// <summary>
        /// Hent infisert sone
        /// </summary>
        /// <returns>Polygon</returns>
        [HttpGet]
        [Route("")]
        [Obsolete]
        public HttpResponseMessage HentSone()
        {
            var gameState = _gameStateService.HentGameState();
            return Request.CreateResponse(HttpStatusCode.OK, gameState.InfisertPolygon);
        }

        // POST api/infisert
        /// <summary>
        /// Setter infisert sone
        /// </summary>
        /// <param name="modell"></param>
        [HttpPost]
        [Route("")]
        [Obsolete] // Skjule for Swagger-apidoc
        public async Task<HttpResponseMessage> Post([FromBody] InfisertPolygon modell)
        {
            var gameState = _gameStateService.HentGameState();
            gameState.InfisertPolygon = modell;

            try
            {
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