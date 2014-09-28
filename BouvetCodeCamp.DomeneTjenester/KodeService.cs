using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using BouvetCodeCamp.Domene;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester.Interfaces;

namespace BouvetCodeCamp.DomeneTjenester
{
    public class KodeService : IKodeService
    {
        private readonly ILagService _lagService;
        private readonly IKoordinatVerifier koordinatVerifier;

        public KodeService(ILagService lagService, IKoordinatVerifier koordinatVerifier)
        {
            _lagService = lagService;
            this.koordinatVerifier = koordinatVerifier;
        }

        public IEnumerable<Kode> HentOppdagedeKoder(string lagId)
        {
            var lag = _lagService.HentLagMedLagId(lagId);

            return lag.Koder.Where(kode => kode.PosisjonTilstand.Equals(PosisjonTilstand.Oppdaget));
        }

        public IEnumerable<Kode> HentAlleKoder(string lagId)
        {
            var lag = _lagService.HentLagMedLagId(lagId);

            return lag.Koder;
        }

        /// <summary>
        /// Finn et lag sin kode og sett tilstanden til oppdaget, søket går på følgende kriterier:
        ///   1 - bokstav stemmer
        ///   2 - koordinatene er innenfor rekkevidde
        ///   3 - tilstanden ikke allerede er satt til oppdaget
        /// </summary>
        /// <returns>true hvis alle kriterier er oppfylt</returns>
        public bool SettKodeTilstandTilOppdaget(string lagId, string kode, Koordinat koordinat)
        {
            var lag = _lagService.HentLagMedLagId(lagId);

            var kandidater = lag.Koder.Where(k => k.Bokstav.Equals(kode, StringComparison.CurrentCultureIgnoreCase)
                && this.koordinatVerifier.KoordinaterErNærHverandre(k.Posisjon, koordinat)
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
