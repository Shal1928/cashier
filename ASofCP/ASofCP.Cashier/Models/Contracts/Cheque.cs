using System;

// ReSharper disable CheckNamespace
namespace it.q02.asocp.api.data
// ReSharper restore CheckNamespace
{
    public class Cheque
    {
        public long Id;

        public Shift Shift;

        public DateTime OpenDate;

        public DateTime CloseDate;

        public short MoneyType;

        public ChequeRow[] Rows;


        public override String ToString() 
        {
            return "Cheque{" +
                    "Id=" + Id +
                    ", Shift=" + Shift +
                    ", OpenDate=" + OpenDate +
                    ", CloseDate=" + CloseDate +
                    ", MoneyType='" + MoneyType + '\'' +
                    ", Rows=" + Rows +
                    '}';
        }
    }
}
