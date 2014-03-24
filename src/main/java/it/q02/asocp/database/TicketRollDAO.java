package it.q02.asocp.database;

import it.q02.asocp.modules.base.client.data.TicketRoll;

import java.util.List;

/**
 * User: aleksander at  24.03.14, 16:36
 */
public interface TicketRollDAO {

    public void create(TicketRoll ticketRoll);
    public void update(TicketRoll ticketRoll);

    public List<TicketRoll> find();

    public List<String> getUniqueColors();


}
