using System;

// ReSharper disable CheckNamespace
namespace it.q02.asocp.api.data
// ReSharper restore CheckNamespace
{
    public class Shift
    {
        public long Id;

        public String CashierName;

        public bool Active;

        public DateTime OpenDate;

        public DateTime CloseDate;


        public override String ToString()
        {
            return "Shift{" +
                    "Id=" + Id +
                    ", CashierName='" + CashierName + '\'' +
                    ", Active=" + Active +
                    ", OpenDate=" + OpenDate +
                    ", CloseDate=" + CloseDate +
                    '}';
        }
    }
}
