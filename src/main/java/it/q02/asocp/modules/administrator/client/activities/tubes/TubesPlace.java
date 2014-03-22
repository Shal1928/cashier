package it.q02.asocp.modules.administrator.client.activities.tubes;

import com.google.gwt.place.shared.Place;
import com.google.gwt.place.shared.PlaceTokenizer;

/**
 * User: aleksander at  22.03.14, 18:49
 */
public class TubesPlace extends Place {

    private boolean editing;


    public static class Tokenizer implements PlaceTokenizer<TubesPlace> {

        @Override
        public TubesPlace getPlace(String s) {
            return new TubesPlace();
        }

        @Override
        public String getToken(TubesPlace ticketPlace) {
            return "";
        }

    }


}
