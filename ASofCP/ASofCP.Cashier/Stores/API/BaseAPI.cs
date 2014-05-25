using System;
using it.q02.asocp.api.data;

namespace ASofCP.Cashier.Stores.API
{
    // ReSharper disable InconsistentNaming
    public interface BaseAPI
    {
        AttractionGroupInfo[] getGroups();

        AttractionInfo[] getAttractionsFromGroup(AttractionGroupInfo group);

        AttractionInfo[] findAttractionBy(String mask);

        RollInfo activateTicketRoll(String series, long nextTicket, RollColor color);

        bool deactivateTicketRoll(String series, long nextTicket, RollColor color);

        bool closeTicketRoll(RollInfo info);

        RollColor[] getColors();

        bool isShiftOpen();

        Shift getCurrentShift();

        Shift openShift();

        void closeShift(Shift shift);

        long getNextChequeNumber();

        long createCheque(Cheque cheque);

        TicketInfo findTicketByBarcode(String ticketBarcode);

        TicketInfo reverseTicket(TicketInfo ticketInfo, String reverseReason);
    }
    // ReSharper restore InconsistentNaming
}
