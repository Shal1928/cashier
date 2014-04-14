using System;
using ASofCP.Cashier.Models.Contracts;

// ReSharper disable CheckNamespace
namespace it.q02.asocp.api.data
// ReSharper restore CheckNamespace
{
    public class ChequeRow
    {

        public long Id;

        public AttractionInfo Attraction;

        public RollInfo TicketRoll;

        public long TicketNumber;

        public String TicketBarCode;

        public bool Printed;

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
