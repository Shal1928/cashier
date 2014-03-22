package it.q02.asocp.modules.administrator.client.activities.tubes;

import com.google.gwt.activity.shared.Activity;
import com.google.gwt.event.shared.EventBus;
import com.google.gwt.user.client.ui.AcceptsOneWidget;
import com.google.gwt.user.client.ui.Label;
import it.q02.asocp.modules.administrator.client.ui.menu.bus.ClearMenu;

/**
 * User: aleksander at  22.03.14, 18:35
 */
public class TubesActivity implements Activity {

    private EventBus eventBus;

    @Override
    public String mayStop() {
        return null;
    }

    @Override
    public void onCancel() {

    }

    @Override
    public void onStop() {
        eventBus.fireEvent(new ClearMenu());
    }

    @Override
    public void start(AcceptsOneWidget acceptsOneWidget, EventBus eventBus) {
        this.eventBus = eventBus;
        acceptsOneWidget.setWidget(new Label("HUI!"));
    }
}
