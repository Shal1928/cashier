package it.q02.asocp.modules.administrator.client.rpc;

import com.google.gwt.user.client.rpc.RemoteService;
import com.google.gwt.user.client.rpc.RemoteServiceRelativePath;
import com.google.gwt.core.client.GWT;
import it.q02.asocp.modules.base.client.data.TicketRoll;

import java.util.List;

/**
 * User: aleksander at  24.03.14, 17:06
 */
@RemoteServiceRelativePath("TicketRollService")
public interface TicketRollService extends RemoteService {

    public TicketRoll saveAndUpdate(TicketRoll ticketRoll );

    public List<TicketRoll> findTicketRolls();

    /**
     * Utility/Convenience class.
     * Use TicketRollService.App.getInstance() to access static instance of TicketRollServiceAsync
     */
    public static class App {
        private static final TicketRollServiceAsync ourInstance = (TicketRollServiceAsync) GWT.create(TicketRollService.class);

        public static TicketRollServiceAsync getInstance() {
            return ourInstance;
        }
    }


}
