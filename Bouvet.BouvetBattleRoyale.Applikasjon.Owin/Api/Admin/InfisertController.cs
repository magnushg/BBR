namespace Bouvet.BouvetBattleRoyale.Applikasjon.Owin.Api.Admin
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Bouvet.BouvetBattleRoyale.Applikasjon.Owin;
    using Bouvet.BouvetBattleRoyale.Domene;
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Domene.OutputModels;
    using Bouvet.BouvetBattleRoyale.Tjenester.Interfaces;
    using Bouvet.BouvetBattleRoyale.Tjenester.SignalR.Hubs;

    using BouvetCodeCamp.Domene;
    using BouvetCodeCamp.Domene.Entiteter;

    /// <summary>
    /// Sett og Hent infiserte soner
    /// </summary>
    [RoutePrefix("api/admin/infisert")]
    [System.Web.Http.Authorize]
    public class InfisertController : BaseApiController
    {
        private readonly IService<GameState> _gameStateService;

        private readonly IGameHub _gameHub;

        public InfisertController(
            IService<GameState> gameStateService,
            IGameHub gameHub)
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
            var gameState = _gameStateService.Hent(string.Empty);

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

            var gameState = _gameStateService.Hent(string.Empty);
            gameState.InfisertPolygon = modell;

            try
            {
                _gameHub.SettInfisertSone(
                    new InfisertPolygonOutputModell
                    {
                        Koordinater = modell.Koordinater
                    });

                await _gameStateService.Oppdater(gameState);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}