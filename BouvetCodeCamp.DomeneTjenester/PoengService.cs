using System;
using System.Linq;
using BouvetCodeCamp.Domene;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester.Interfaces;

namespace BouvetCodeCamp.DomeneTjenester
{
    using BouvetCodeCamp.Domene.OutputModels;
    using BouvetCodeCamp.SignalR.Hubs;

    using Microsoft.AspNet.SignalR;

    public class PoengService : IPoengService
    {
        private readonly PoengTildeling _poengTildeling;

        private readonly Lazy<IHubContext<IGameHub>> gameHub;

        public PoengService(PoengTildeling poengTildeling, Lazy<IHubContext<IGameHub>> gameHub)
        {
            _poengTildeling = poengTildeling;
            this.gameHub = gameHub;
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

                var sekunderOverTimeOut = timeSpan.TotalSeconds - _poengTildeling.PingTimeout;

                if (sekunderOverTimeOut > 0)
                {
                    var poeng = _poengTildeling.PingTimeoutStraff;
                    
                    lag.Poeng += poeng;

                    lag.LoggHendelser.Add(new LoggHendelse
                    {
                        HendelseType = HendelseType.PingPoengTap,
                        Kommentar = String.Format("Ping timeout, {0} i straffepoeng", _poengTildeling.PingTimeoutStraff),
                        Tid = DateTime.Now
                    });

                    SendTildeltPoengHendelse(lag, poeng, HendelseType.PingPoengTap);
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

                var poeng = (int)Math.Round(poengTap);
                
                lag.Poeng += poeng;

                lag.LoggHendelser.Add(new LoggHendelse
                {
                    HendelseType = HendelseType.InfisertPoengTap,
                    Kommentar = String.Format("Mistet {0} poeng for å ha vært infisert i {1} sekund", poengTap, infisertTimeSpan),
                    Tid = DateTime.Now
                });

                SendTildeltPoengHendelse(lag, poeng, HendelseType.InfisertPoengTap);
            }

            return lag;
        }

        public Lag SettMeldingSendtStraff(Lag lag, Melding melding)
        {
            var poeng = _poengTildeling.MeldingsStraff;

            lag.Poeng += poeng;

            SendTildeltPoengHendelse(lag, poeng, HendelseType.SendtFritekstmeldingStraff);

            return lag;
        }

        public Lag SettPoengForKodeRegistrert(Lag lag, HendelseType hendelse)
        {
            if (hendelse.Equals(HendelseType.RegistrertKodeSuksess))
            {
                var poeng = _poengTildeling.KodeOppdaget;

                lag.Poeng += poeng;

                SendTildeltPoengHendelse(lag, poeng, HendelseType.RegistrertKodeSuksess);
            }
            return lag;
        }

        public Lag SettPoengForLag(Lag lag, int poeng, string kommentar)
        {
            lag.Poeng += _poengTildeling.MeldingsStraff;

            SendTildeltPoengHendelse(lag, poeng, HendelseType.TildeltPoeng);

            return lag;
        }

        private void SendTildeltPoengHendelse(Lag lag, int poeng, HendelseType hendelseType)
        {
            gameHub.Value.Clients.All.NyLoggHendelse(new LoggHendelseOutputModell {
                                                             Hendelse = HendelseTypeFormatter.HentTekst(hendelseType) + ": " + poeng + " poeng. " + lag.LagNavn + " har nå " + lag.Poeng + " poeng",
                                                             LagId = lag.LagId,
                                                             Tid = DateTime.Now.ToShortTimeString()
                                                         });
        }
    }
}