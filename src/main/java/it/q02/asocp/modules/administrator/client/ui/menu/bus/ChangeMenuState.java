package it.q02.asocp.modules.administrator.client.ui.menu.bus;

import com.google.gwt.event.shared.GwtEvent;
import it.q02.asocp.modules.administrator.client.ui.menu.item.MenuItem;

/**
 * User: aleksander at  22.03.14, 22:18
 */
public class ChangeMenuState extends GwtEvent<ChangeMenuStateHandler> {
    public static enum State{
        ENABLED,DISABLED;
    }



    private State newState;
    private ApplyToFilter filter;

    public ChangeMenuState(State newState, ApplyToFilter filter) {
        this.newState = newState;
        this.filter = filter;
    }

    public State getNewState() {
        return newState;
    }

    public ApplyToFilter getFilter() {
        return filter;
    }

    public static Type<ChangeMenuStateHandler> TYPE = new Type<ChangeMenuStateHandler>();

    public Type<ChangeMenuStateHandler> getAssociatedType() {
        return TYPE;
    }

    protected void dispatch(ChangeMenuStateHandler handler) {
        handler.onChangeMenuState(this);
    }
}
