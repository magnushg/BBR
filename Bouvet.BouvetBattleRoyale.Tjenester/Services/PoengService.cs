namespace Bouvet.BouvetBattleRoyale.Tjenester.Services
{
    using System;
    using System.Linq;

    using Bouvet.BouvetBattleRoyale.Domene;
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Domene.OutputModels;
    using Bouvet.BouvetBattleRoyale.Infrastruktur.Interfaces;
    using Bouvet.BouvetBattleRoyale.Tjenester.Interfaces;
    using Bouvet.BouvetBattleRoyale.Tjenester.Interfaces.Services;
    using Bouvet.BouvetBattleRoyale.Tjenester.Interfaces.SignalR.Hubs;

    public class PoengService : IPoengService
    {
        private readonly IGameHub _gameHub;

        private readonly IArkivHandler _arkivHandler;

        public PoengService(IGameHub gameHub, IArkivHandler arkivHandler)
        {
            _gameHub = gameHub;
            _arkivHandler = arkivHandler;
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
                        LagId = lag.LagId,
                        HendelseType = HendelseType.PingPoengTap,
                        Kommentar = String.Format("Ping timeout, {0} i straffepoeng", PoengTildeling.PingTimeoutStraff),
                        Tid = DateTime.Now
                    };
                    
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
                                           LagId = lag.LagId,
                                           HendelseType = HendelseType.InfisertPoengTap,
                                           Kommentar =
                                               String.Format(
                                                   "Mistet {0} poeng for å ha vært infisert i {1:0} sekunder",
                                                   poeng,
                                                   infisertTimeSpan.TotalSeconds),
                                           Tid = DateTime.Now
                                       };
                
                SendTildeltPoengHendelse(lag, loggHendelse, lag.Poeng);
            }

            return lag;
        }

        public Lag SettFritekstMeldingSendtStraff(Lag lag, Melding melding)
        {
            if (!melding.Type.Equals(MeldingType.Fritekst)) return lag;

            var poeng = PoengTildeling.MeldingsStraff;

            var antallBokstaver = melding.Tekst.Length;

            lag.Poeng += (poeng * antallBokstaver);

            var loggHendelse = new LoggHendelse
                                   {
                                       LagId = lag.LagId,
                                       HendelseType = HendelseType.SendtFritekstmeldingStraff,
                                       Tid = DateTime.Now,
                                       Kommentar = string.Format("{0} poeng tapt for fritekst", poeng)
                                   };
            
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
                                           LagId = lag.LagId,
                                           HendelseType = HendelseType.RegistrertKodeSuksess,
                                           Tid = DateTime.Now,
                                           Kommentar = string.Format("{0} poeng for post {1}", poeng, postnummer)
                                       };
                
                SendTildeltPoengHendelse(lag, loggHendelse, lag.Poeng);
            }
            return lag;
        }

        public Lag SettPoengForLag(Lag lag, int poeng, string kommentar)
        {
            lag.Poeng += poeng;

            var loggHendelse = new LoggHendelse
            {
                LagId = lag.LagId,
                HendelseType = HendelseType.TildeltPoeng,
                Tid = DateTime.Now,
                Kommentar = string.Format("{0} poeng. {1}", poeng, kommentar)
            };
            
            SendTildeltPoengHendelse(lag, loggHendelse, lag.Poeng);

            return lag;
        }

        private void SendTildeltPoengHendelse(Lag lag, LoggHendelse loggHendelse, int nyPoengsum)
        {
            var loggHendelseOutputModell = new LoggHendelseOutputModell
            {
                Hendelse = HendelseTypeFormatter.HentTekst(loggHendelse.HendelseType),
                Kommentar = loggHendelse.Kommentar,
                LagNummer = lag.LagNummer,
                Tid = DateTime.Now.ToLongTimeString(),
                LagId = lag.LagId
            };

            _arkivHandler.SendTilArkivet(loggHendelse);

            _gameHub.NyLoggHendelse(loggHendelseOutputModell);

            _gameHub.PoengTildelt(
                new PoengOutputModell
                    {
                        LagId = lag.LagId,
                        NyPoengsum = nyPoengsum
                    });
        }
    }
}
