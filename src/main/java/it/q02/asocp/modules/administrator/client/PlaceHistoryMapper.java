package it.q02.asocp.modules.administrator.client;

import com.google.gwt.place.shared.WithTokenizers;
import it.q02.asocp.modules.administrator.client.activities.tickets.TicketPlace;
import it.q02.asocp.modules.administrator.client.activities.tubes.TubesPlace;

/**
 * User: aleksander at  22.03.14, 18:13
 */
@WithTokenizers({TicketPlace.Tokenizer.class, TubesPlace.Tokenizer.class})
public interface PlaceHistoryMapper extends com.google.gwt.place.shared.PlaceHistoryMapper {
}
