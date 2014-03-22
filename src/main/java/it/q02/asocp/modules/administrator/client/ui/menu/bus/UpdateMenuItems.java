package it.q02.asocp.modules.administrator.client.ui.menu.bus;

import com.google.gwt.event.shared.GwtEvent;

/**
 * User: aleksander at  22.03.14, 22:28
 */
public class UpdateMenuItems extends GwtEvent<UpdateMenuItemsHandler> {
    public static Type<UpdateMenuItemsHandler> TYPE = new Type<UpdateMenuItemsHandler>();

    private ApplyToFilter filter;

    public UpdateMenuItems(ApplyToFilter filter) {
        this.filter = filter;
    }

    public ApplyToFilter getFilter() {
        return filter;
    }

    public void setFilter(ApplyToFilter filter) {
        this.filter = filter;
    }

    public Type<UpdateMenuItemsHandler> getAssociatedType() {
        return TYPE;
    }

    protected void dispatch(UpdateMenuItemsHandler handler) {
        handler.onUpdateMenuItems(this);
    }
}
