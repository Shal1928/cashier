package it.q02.asocp.modules.administrator.client.rpc;

import com.google.gwt.user.client.rpc.IsSerializable;
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

    public static enum Error{
        NOT_UNIQUE_BARCODE("Не уникальный штрих код"),DONT_HAVE_RIGHTS("Не достаточно прав"),OBJECT_NOT_EDITABLE("Нельзя изменить.");
        private final String errorDescribe;

        Error(String s) {
            this.errorDescribe = s;
        }

        public String getErrorDescribe() {
            return errorDescribe;
        }
    }

    public static class TicketRollServiceException extends Exception  {

        private Error error;

        public TicketRollServiceException() {
        }

        public TicketRollServiceException(String message,Error error) {
            super(message);
            this.error = error;
        }

        public TicketRollServiceException(String message,Error error, Throwable cause) {
            super(message, cause);
            this.error = error;
        }

        public Error getError() {
            return error;
        }
    }

    public TicketRoll saveAndUpdate(TicketRoll ticketRoll ) throws TicketRollServiceException;

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
