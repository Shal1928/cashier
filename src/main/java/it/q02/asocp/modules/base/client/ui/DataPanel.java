package it.q02.asocp.modules.base.client.ui;

import com.google.gwt.safehtml.shared.SafeHtml;
import it.q02.asocp.modules.base.client.ui.ChangeCommand;

/**
 * User: aleksander at  18.03.14, 17:45
 * Панель с данными
 */
public interface DataPanel<T> extends HasSelection<T> {

    public static enum State{
        WAIT_DATA,IDLE
    }

    public static interface PanelStateListener{

        public void onStateChange(DataPanel panel,State state);
        public void showStatusText(SafeHtml statusText);
    }

    public void addPanelStateListener(PanelStateListener listener);

    public void commitChanged(ChangeCommand cmd,T object);
    public void cancelChanged(ChangeCommand cmd,T object);

}
