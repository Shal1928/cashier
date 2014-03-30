package it.q02.asocp.modules.base.client.ui.editor;

import com.google.gwt.user.client.ui.IsWidget;
import com.google.gwt.user.client.ui.Widget;
import it.q02.asocp.modules.base.client.data.TicketRoll;
import it.q02.asocp.modules.base.client.ui.EditorStateCallback;

/**
 * User: aleksander at  29.03.14, 20:58
 */
public interface BaseEditor<T> extends IsWidget {

    void setValue(T ticketRoll, EditorStateCallback<T> callBack);

    T getValue();

    @Override
    Widget asWidget();
}
