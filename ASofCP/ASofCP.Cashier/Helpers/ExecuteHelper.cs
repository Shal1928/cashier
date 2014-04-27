using System;
using System.Text;
using log4net;

namespace ASofCP.Cashier.Helpers
{
    public static class ExecuteHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ExecuteHelper));

        public static void Try(Action<object> execute, bool isThrow = true)
        {
            Try<Exception>(execute, isThrow);
        }

        public static TResult Try<TResult>(Func<object, TResult> execute, bool isThrow = true)
        {
            return Try<TResult, Exception>(execute, isThrow);
        }

        public static void Try<T>(Action<object> execute, bool isThrow = true) where T : Exception
        {
            try
            {
                execute.Invoke(null);
            }
            catch (T e)
            {
                ExceptionToLog(e);
                if(isThrow) throw;
            }
        }

        public static TResult Try<TResult, TException>(Func<object, TResult> execute, bool isThrow = true) where TException : Exception
        {
            try
            {
                return execute.Invoke(null);
            }
            catch (TException e)
            {
                ExceptionToLog(e);
                if (isThrow) throw;
            }

            return default(TResult);
        }

        public static void ExceptionToLog(Exception e)
        {
            if (e == null)
            {
                Log.Fatal("Исключение не определено!");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("Произошло исключение: {0}".F(e.Message));
            sb.AppendLine("Внутреннее исключение:");
            sb.AppendLine(Convert.ToString(e.InnerException));
            sb.AppendLine("StackTrace:");
            sb.AppendLine(e.StackTrace);
            Log.Fatal(sb);
        }
    }
}
