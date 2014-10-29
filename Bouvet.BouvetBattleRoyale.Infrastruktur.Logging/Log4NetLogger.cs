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
    }
}