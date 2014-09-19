using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BouvetCodeCamp.Felles;
using BouvetCodeCamp.Felles.Entiteter;
using BouvetCodeCamp.Felles.Interfaces;
using BouvetCodeCamp.Service.Interfaces;

namespace BouvetCodeCamp.Service.Services
{
    public class KodeService : IKodeService
    {
        private readonly ILagService _lagService;
        private readonly ICoordinateVerifier _coordinateVerifier;

        public KodeService(ILagService lagService, ICoordinateVerifier coordinateVerifier)
        {
            _lagService = lagService;
            _coordinateVerifier = coordinateVerifier;
        }

        public async Task<IEnumerable<Kode>> HentOppdagetKoder(string lagId)
        {
            var lag = await _lagService.HentLag(lagId);

            return lag.Koder.Where(kode => kode.PosisjonTilstand.Equals(PosisjonTilstand.Oppdaget));
        }

        public async Task<IEnumerable<Kode>> HentAlleKoder(string lagId)
        {
            var lag = await _lagService.HentLag(lagId);

            return lag.Koder;
        }

        /// <summary>
        /// Finn et lag sin kode og sett tilstanden til oppdaget, søket går på følgende kriterier:
        ///   1 - bokstav stemmer
        ///   2 - koordinatene er innenfor rekkevidde
        ///   3 - tilstanden ikke allerede er satt til oppdaget
        /// </summary>
        /// <returns>true hvis alle kriterier er oppfylt</returns>
        public async Task<bool> SettKodeTilstandTilOppdaget(string lagId, string kode, Coordinate koordinat)
        {
            var lag = await _lagService.HentLag(lagId);

            var kandidater = lag.Koder.Where(k => k.Bokstav.Equals(kode, StringComparison.CurrentCultureIgnoreCase)
                && _coordinateVerifier.CoordinateSAreInProximity(k.Gps, koordinat)
                && k.PosisjonTilstand.Equals(PosisjonTilstand.Ukjent)).ToList();

            switch (kandidater.Count())
            {
                case 0:
                    return false;
                case 1:
                    kandidater.First().PosisjonTilstand = PosisjonTilstand.Oppdaget;
                    _lagService.Oppdater(lag);
                    return true;
                default:
                    throw new AmbiguousMatchException("Flere koder funnet basert på kriteriene gitt");
            }
        }
    }
}
