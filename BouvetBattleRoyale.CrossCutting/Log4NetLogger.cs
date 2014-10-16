namespace BouvetCodeCamp.CrossCutting
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