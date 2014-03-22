package it.q02.asocp.modules.administrator.client.activities.tickets;

import com.google.gwt.place.shared.Place;
import com.google.gwt.place.shared.PlaceTokenizer;

/**
 * User: aleksander at  22.03.14, 18:37
 */
public class TicketPlace extends Place {

    private boolean editing;


    public static class Tokenizer implements PlaceTokenizer<TicketPlace>{

        @Override
        public TicketPlace getPlace(String s) {
            return new TicketPlace();
        }

        @Override
        public String getToken(TicketPlace ticketPlace) {
            return "";
        }

    }



}
