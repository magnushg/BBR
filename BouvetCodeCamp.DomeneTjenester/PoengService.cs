using System;
using System.Linq;
using BouvetCodeCamp.Domene;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester.Interfaces;

namespace BouvetCodeCamp.DomeneTjenester
{
    using Domene.OutputModels;
    using SignalR.Hubs;

    using Microsoft.AspNet.SignalR;

    public class PoengService : IPoengService
    {
        private readonly IGameHub _gameHub;

        public PoengService(IGameHub gameHub)
        {
            _gameHub = gameHub;
        }

        public Lag SjekkOgSettPifPingStraff(Lag lag)
        {
            var pifPosisjons = lag.PifPosisjoner
                .OrderByDescending(x => x.Tid)
                .Take(2)
                .ToList();

            if (pifPosisjons.Count() == 2)
            {
                TimeSpan timeSpan = pifPosisjons.First().Tid.Subtract(pifPosisjons.Last().Tid);

                var sekunderOverTimeOut = timeSpan.TotalSeconds - PoengTildeling.PingTimeout;

                if (sekunderOverTimeOut > 0)
                {
                    var poeng = PoengTildeling.PingTimeoutStraff;

                    lag.Poeng += poeng;

                    var loggHendelse = new LoggHendelse
                    {
                        HendelseType = HendelseType.PingPoengTap,
                        Kommentar = String.Format("Ping timeout, {0} i straffepoeng", PoengTildeling.PingTimeoutStraff),
                        Tid = DateTime.Now
                    };

                    lag.LoggHendelser.Add(loggHendelse);

                    SendTildeltPoengHendelse(lag, loggHendelse, lag.Poeng);
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

                var poengTap = infisertTimeSpan.TotalSeconds * PoengTildeling.InfisertTickStraff;

                var poeng = (int)Math.Round(poengTap);

                lag.Poeng += poeng;

                var loggHendelse = new LoggHendelse
                                       {
                                           HendelseType = HendelseType.InfisertPoengTap,
                                           Kommentar =
                                               String.Format(
                                                   "Mistet {0} poeng for å ha vært infisert i {1:0} sekunder",
                                                   poeng,
                                                   infisertTimeSpan.TotalSeconds),
                                           Tid = DateTime.Now
                                       };

                lag.LoggHendelser.Add(loggHendelse);

                SendTildeltPoengHendelse(lag, loggHendelse, lag.Poeng);
            }

            return lag;
        }

        public Lag SettMeldingSendtStraff(Lag lag, Melding melding)
        {
            var poeng = PoengTildeling.MeldingsStraff;

            lag.Poeng += poeng;

            var loggHendelse = new LoggHendelse
                                   {
                                       HendelseType = HendelseType.SendtFritekstmeldingStraff, 
                                       Tid = DateTime.Now,
                                       Kommentar = string.Format("{0} poeng", poeng)
                                   };

            lag.LoggHendelser.Add(loggHendelse);

            SendTildeltPoengHendelse(lag, loggHendelse, lag.Poeng);

            return lag;
        }

        public Lag SettPoengForKodeRegistrert(Lag lag, HendelseType hendelse, int postnummer)
        {
            if (hendelse.Equals(HendelseType.RegistrertKodeSuksess))
            {
                var poeng = PoengTildeling.KodeOppdaget;

                lag.Poeng += poeng;

                var loggHendelse = new LoggHendelse
                                       {
                                           HendelseType = HendelseType.RegistrertKodeSuksess, 
                                           Tid = DateTime.Now,
                                           Kommentar = string.Format("{0} poeng for post {1}", poeng, postnummer)
                                       };

                lag.LoggHendelser.Add(loggHendelse);
                
                SendTildeltPoengHendelse(lag, loggHendelse, lag.Poeng);
            }
            return lag;
        }
        
        public Lag SettPoengForLag(Lag lag, int poeng, string kommentar)
        {
            lag.Poeng += poeng;

            var loggHendelse = new LoggHendelse { 
                HendelseType = HendelseType.TildeltPoeng, 
                Tid = DateTime.Now,
                Kommentar = string.Format("{0} poeng. {1}", poeng, kommentar)
            };

            lag.LoggHendelser.Add(loggHendelse);

            SendTildeltPoengHendelse(lag, loggHendelse, lag.Poeng);

            return lag;
        }

        private void SendTildeltPoengHendelse(Lag lag, LoggHendelse loggHendelse, int nyPoengsum)
        {
            var loggHendelseOutputModell = new LoggHendelseOutputModell {
                                                   Hendelse = HendelseTypeFormatter.HentTekst(loggHendelse.HendelseType),
                                                   Kommentar = loggHendelse.Kommentar,
                                                   LagNummer = lag.LagNummer,
                                                   Tid = DateTime.Now.ToLongTimeString()
                                               };

            // TODO: Må fikse mocking slik at testene ikke trenger en try-catch for å ikke feile.
            //try
            //{
                _gameHub.NyLoggHendelse(loggHendelseOutputModell);

                _gameHub.PoengTildelt(
                    new PoengOutputModell { LagId = lag.LagId, NyPoengsum = nyPoengsum });
            //}
            //catch (Exception)
            //{
                
            //}
        }
    }
}
