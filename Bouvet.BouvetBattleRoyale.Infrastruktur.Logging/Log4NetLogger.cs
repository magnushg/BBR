namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Logging
{
    using System;

    using log4net;

    public static class Log4NetLogger
    {
        public static ILog HentLogger(Type T)
        {
            return LogManager.GetLogger(T);
        }
        
        public static void InitialiserLogging<T>()
        {
            log4net.Config.XmlConfigurator.Configure();

            var log = LogManager.GetLogger(typeof(T));

            log.Info("Log4Net initialized.");
        }
    }
}