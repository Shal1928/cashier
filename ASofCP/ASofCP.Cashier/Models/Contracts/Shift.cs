using System;
using System.Runtime.Serialization;

// ReSharper disable CheckNamespace
namespace it.q02.asocp.api.data
// ReSharper restore CheckNamespace
{
    [DataContract]
    public class Shift
    {
        [DataMember]
        public long Id;

        [DataMember]
        public String CashierName;

        [DataMember]
        public bool Active;

        [DataMember]
        public DateTime OpenDate;

        [DataMember]
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
