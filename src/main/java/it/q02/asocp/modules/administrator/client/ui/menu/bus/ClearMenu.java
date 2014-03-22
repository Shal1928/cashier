package it.q02.asocp.modules.administrator.client.ui.menu.bus;

import com.google.gwt.event.shared.GwtEvent;

/**
 * User: aleksander at  22.03.14, 22:09
 */
public class ClearMenu extends GwtEvent<ClearMenuHandler> {
    public static Type<ClearMenuHandler> TYPE = new Type<ClearMenuHandler>();

    public Type<ClearMenuHandler> getAssociatedType() {
        return TYPE;
    }

    protected void dispatch(ClearMenuHandler handler) {
        handler.onClearMenu(this);
    }
}
