package it.q02.asocp.modules.administrator.client.rpc;

import com.google.gwt.user.client.rpc.AsyncCallback;
import it.q02.asocp.modules.base.client.data.TicketRoll;

import java.util.List;

/**
 * User: aleksander at  24.03.14, 17:06
 */
public interface TicketRollServiceAsync {

    void saveAndUpdate(TicketRoll ticketRoll, AsyncCallback<TicketRoll> async);

    void findTicketRolls(AsyncCallback<List<TicketRoll>> async);
}
