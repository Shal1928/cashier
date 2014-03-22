package it.q02.asocp.modules.administrator.client.impl;

import com.google.gwt.event.shared.EventBus;
import com.google.gwt.event.shared.SimpleEventBus;
import com.google.gwt.place.shared.PlaceController;

import com.google.gwt.user.client.ui.AcceptsOneWidget;
import it.q02.asocp.modules.administrator.client.ClientFactory;

/**
 * User: aleksander at  22.03.14, 18:08
 */
public class ClientFactoryImpl implements ClientFactory{

    private EventBus eventBus = new SimpleEventBus();

    private PlaceController controller = new PlaceController(eventBus);



    @Override
    public EventBus getEventBus() {
        return eventBus;
    }

    @Override
    public PlaceController getPlaceController() {
        return controller;
    }

}
