using System;
using System.Runtime.Serialization;

// ReSharper disable CheckNamespace
namespace it.q02.asocp.api.data
// ReSharper restore CheckNamespace
{
    [DataContract]
    public class Cheque
    {
        [DataMember]
        public long Id;

        [DataMember]
        public Shift Shift;

        [DataMember]
        public DateTime OpenDate;

        [DataMember]
        public DateTime CloseDate;

        [DataMember]
        public short MoneyType;

        [DataMember]
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
