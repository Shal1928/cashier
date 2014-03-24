package it.q02.asocp.modules.administrator.server.rpc;

import com.google.gwt.user.server.rpc.RemoteServiceServlet;
import it.q02.asocp.database.DataBaseService;
import it.q02.asocp.database.TicketRollDAO;
import it.q02.asocp.modules.administrator.client.rpc.TicketRollService;
import it.q02.asocp.modules.base.client.data.TicketRoll;
import it.q02.asocp.users.ExecutionContextStorage;

import java.util.List;

/**
 * User: aleksander at  24.03.14, 17:06
 */
public class TicketRollServiceImpl extends RemoteServiceServlet implements TicketRollService {
    @Override
    public TicketRoll saveAndUpdate(TicketRoll ticketRoll) {
        DataBaseService service = ExecutionContextStorage.getContext().getDatabaseService();
        TicketRollDAO dao =service.getMapper(TicketRollDAO.class);
        if(ticketRoll.getId()==0){
              dao.create(ticketRoll);
        }else{
            dao.update(ticketRoll);
        }
        service.commitTransaction();
        return ticketRoll;
    }


    @Override
    public List<TicketRoll> findTicketRolls() {
        return getDAO().find();
    }


    private TicketRollDAO getDAO() {
        DataBaseService service = ExecutionContextStorage.getContext().getDatabaseService();
        return service.getMapper(TicketRollDAO.class);
    }

}