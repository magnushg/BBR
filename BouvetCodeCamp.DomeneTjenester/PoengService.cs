using System;
using System.Linq;
using BouvetCodeCamp.Domene;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester.Interfaces;

namespace BouvetCodeCamp.DomeneTjenester
{
    
    public class PoengService : IPoengService
    {
            //TODO: sett inn signalr event på poengtildeling    
            //lag.LoggHendelser.Add(
            //    new LoggHendelse
            //    {
            //        HendelseType = HendelseType.TildeltPoeng,
            //        Tid = DateTime.Now,
            //        Kommentar = inputModell.Kommentar
            //    });
        private readonly PoengTildeling _poengTildeling;

        public PoengService(PoengTildeling poengTildeling)
        {
            _poengTildeling = poengTildeling;
        }

        public PoengService() : this(new PoengTildeling()) { }

        public Lag SjekkOgSettPifPingStraff(Lag lag)
        {
            var pifPosisjons = lag.PifPosisjoner
                .OrderByDescending(x => x.Tid)
                .Take(2)
                .ToList();

            if (pifPosisjons.Count() == 2)
            {
                TimeSpan timeSpan = pifPosisjons.First().Tid.Subtract(pifPosisjons.Last().Tid);

                var sekunderOverTimeOut = timeSpan.TotalSeconds - _poengTildeling.PingTimeout;

                if (sekunderOverTimeOut > 0)
                {
                    lag.Poeng += _poengTildeling.PingTimeoutStraff;
                    lag.LoggHendelser.Add(new LoggHendelse
                    {
                        HendelseType = HendelseType.PingPoengTap,
                        Kommentar = String.Format("Ping timeout, {0} i straffepoeng", _poengTildeling.PingTimeoutStraff),
                        Tid = DateTime.Now
                    });
                }
            }

            return lag;
        }

        public Lag SjekkOgSettInfisertSoneStraff(Lag lag)
        {
            var pifPosisjons = lag.PifPosisjoner
                .OrderByDescending(x => x.Tid)
                .TakeWhile(x => x.Infisert)
                .Take(2)
                .ToList();

            if (pifPosisjons.Count() == 2)
            {
                TimeSpan infisertTimeSpan = pifPosisjons.First().Tid.Subtract(pifPosisjons.Last().Tid);

                var poengTap = infisertTimeSpan.TotalSeconds * _poengTildeling.InfisertTickStraff;

                lag.Poeng += (int)Math.Round(poengTap);
                lag.LoggHendelser.Add(new LoggHendelse
                {
                    HendelseType = HendelseType.InfisertPoentTap,
                    Kommentar = String.Format("Mistet {0} poeng for å ha vært infisert i {1} sekund", poengTap, infisertTimeSpan),
                    Tid = DateTime.Now
                });
            }

            return lag;
        }

        public Lag SettMeldingSendtStraff(Lag lag, Melding melding)
        {
            lag.Poeng += _poengTildeling.MeldingsStraff;
            return lag;
        }

        public Lag SettPoengForKodeRegistrert(Lag lag, HendelseType hendelse)
        {
            if (hendelse.Equals(HendelseType.RegistrertKodeSuksess)) lag.Poeng += _poengTildeling.KodeOppdaget;
            return lag;
        }

        public Lag SettPoengForLag(Lag lag, int poeng, string kommentar)
        {
            lag.Poeng += _poengTildeling.MeldingsStraff;

            lag.Poeng += _poengTildeling.PingTimeoutStraff;
            lag.LoggHendelser.Add(new LoggHendelse
            {
                HendelseType = HendelseType.TildeltPoeng,
                Kommentar = kommentar,
                Tid = DateTime.Now
            });

            return lag;
        }
    }
}