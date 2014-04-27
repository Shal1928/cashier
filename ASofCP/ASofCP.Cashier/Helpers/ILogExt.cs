using System;
using log4net;

namespace ASofCP.Cashier.Helpers
{
    public static class ILogExt
    {
        public static void Fatal(this ILog log, string message, params object[] parameters)
        {
            log.Fatal(message.F(parameters));
        }

        public static void Fatal(this ILog log, Exception e)
        {
            e.ToLog(log);
        }

        public static void Error(this ILog log, string message, params object[] parameters)
        {
            log.Error(message.F(parameters));
        }

        public static void Warn(this ILog log, string message, params object[] parameters)
        {
            log.Warn(message.F(parameters));
        }

        public static void Debug(this ILog log, string message, params object[] parameters)
        {
            log.Debug(message.F(parameters));
        }

        public static void Info(this ILog log, string message, params object[] parameters)
        {
            log.Info(message.F(parameters));
        }
    }
}
