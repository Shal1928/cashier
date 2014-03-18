package it.q02.asocp.modules.base.client.ui;

import com.google.gwt.user.client.ui.IsWidget;

/**
 * User: aleksander at  18.03.14, 18:07
 * Базовый интерфейс для редакторов.
 * при изменении состояния должен информировать подписчиков об этом.
 *
 *
 */
public interface Editor extends IsWidget{

    public static enum State{
        EDIT,IDLE,CHANGED,SAVING,SAVED,CANCELING,CANCEL;
    }

    public static interface HasStateHandler{

        public void onStateChanged(Editor editor,State state);

    }

    public void addStateHandler(HasStateHandler handler);

}
