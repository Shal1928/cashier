package it.q02.asocp.modules.administrator.client;

import com.google.gwt.event.shared.EventBus;
import com.google.gwt.place.shared.PlaceController;
import com.google.gwt.user.client.ui.AcceptsOneWidget;


/**
 * User: aleksander at  22.03.14, 18:07
 */
public interface ClientFactory {

    public EventBus getEventBus();
    public PlaceController getPlaceController();


}
