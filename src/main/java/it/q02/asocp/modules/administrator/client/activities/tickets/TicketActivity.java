package it.q02.asocp.modules.administrator.client.activities.tickets;

import com.google.gwt.activity.shared.Activity;
import com.google.gwt.event.shared.EventBus;
import com.google.gwt.user.client.Window;
import com.google.gwt.user.client.ui.AcceptsOneWidget;
import com.google.gwt.user.client.ui.Label;
import it.q02.asocp.modules.administrator.client.ui.menu.bus.AddMenu;
import it.q02.asocp.modules.administrator.client.ui.menu.bus.ClearMenu;
import it.q02.asocp.modules.administrator.client.ui.menu.item.AbstractMenu;

/**
 * User: aleksander at  22.03.14, 18:35
 */
public class TicketActivity implements Activity {

    private TicketView view;
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
        this.view = new TicketView(this);
        acceptsOneWidget.setWidget(this.view);
        eventBus.fireEvent(new AddMenu(new AbstractMenu("Создать","create") {
            @Override
            public void execute() {
                Window.alert("Create!");
            }
        }));
    }
}
