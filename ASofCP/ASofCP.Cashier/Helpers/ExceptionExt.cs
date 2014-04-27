using System;
using System.Text;
using log4net;

namespace ASofCP.Cashier.Helpers
{
    public static class ExceptionExt
    {
        public static void ToLog(this Exception e, ILog log)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Произошло исключение: {0}".F(e.Message));
            sb.AppendLine("Внутреннее исключение:");
            sb.AppendLine(Convert.ToString(e.InnerException));
            sb.AppendLine("StackTrace:");
            sb.AppendLine(e.StackTrace);
            log.Fatal(sb);            
        }
    }
}
