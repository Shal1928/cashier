using System;
using System.Runtime.Serialization;

// ReSharper disable CheckNamespace
namespace it.q02.asocp.api.data
// ReSharper restore CheckNamespace
{
    [DataContract]
    public class ChequeRow
    {
        [DataMember]
        public long Id;

        [DataMember]
        public AttractionInfo Attraction;

        [DataMember]
        public RollInfo TicketRoll;

        [DataMember]
        public long TicketNumber;

        [DataMember]
        public String TicketBarCode;

        [DataMember]
        public bool Printed;

        [DataMember]
        public DateTime PrintDate;

        public override String ToString() {
            return "ChequeRow{" +
                    "Id=" + Id +
                    ", Attraction=" + Attraction +
                    ", TicketRoll=" + TicketRoll +
                    ", TicketNumber=" + TicketNumber +
                    ", TicketBarCode='" + TicketBarCode + '\'' +
                    ", Printed=" + Printed +
                    ", PrintDate=" + PrintDate +
                    '}';
        }
    }
}
