package it.q02.asocp.modules.administrator.client;

import com.google.gwt.activity.shared.Activity;
import com.google.gwt.place.shared.Place;
import it.q02.asocp.modules.administrator.client.activities.tickets.TicketActivity;
import it.q02.asocp.modules.administrator.client.activities.tickets.TicketPlace;
import it.q02.asocp.modules.administrator.client.activities.tubes.TubesActivity;
import it.q02.asocp.modules.administrator.client.activities.tubes.TubesPlace;
import it.q02.asocp.modules.administrator.client.ui.layout.MainLayout;

import java.util.HashMap;

/**
 * User: aleksander at  22.03.14, 18:14
 */
public class ActivityMapper implements com.google.gwt.activity.shared.ActivityMapper {

    private HashMap<String,Activity> tokenMap = new HashMap<>();

    private final ClientFactory clientFactory;

    private MainLayout layout;

    public ActivityMapper(ClientFactory factory,MainLayout layout) {
        super();
        this.clientFactory = factory;
        this.layout = layout;
    }

    @Override
    public Activity getActivity(Place place) {
        String placeName = place.getClass().getName();
        layout.getSideBar().setActiveMenu(placeName);
        //todo: Сделать фабрику
        //todo: Сделать привязку к ролям
        if(place instanceof TicketPlace){
            if(tokenMap.containsKey(placeName)){
                return tokenMap.get(placeName);
            }else{
                Activity activity = new TicketActivity();
                tokenMap.put(placeName,activity);
                return activity;
            }
        } else if(place instanceof TubesPlace){
            if(tokenMap.containsKey(placeName)){
                return tokenMap.get(placeName);
            }else{
                Activity activity = new TubesActivity();
                tokenMap.put(placeName,activity);
                return activity;
            }
        }
        return null;
    }

}
