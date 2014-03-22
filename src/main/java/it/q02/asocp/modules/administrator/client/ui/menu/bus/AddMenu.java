package it.q02.asocp.modules.administrator.client.ui.menu.bus;

import com.google.gwt.event.shared.GwtEvent;
import it.q02.asocp.modules.administrator.client.ui.menu.item.MenuItem;

/**
 * User: aleksander at  22.03.14, 22:09
 */
public class AddMenu extends GwtEvent<AddMenuHandler> {

    private MenuItem item;

    public AddMenu(MenuItem item) {
        this.item = item;
    }

    public static Type<AddMenuHandler> TYPE = new Type<AddMenuHandler>();

    public Type<AddMenuHandler> getAssociatedType() {
        return TYPE;
    }

    public MenuItem getItem() {
        return item;
    }

    public void setItem(MenuItem item) {
        this.item = item;
    }

    protected void dispatch(AddMenuHandler handler) {
        handler.onAddMenu(this);
    }
}
