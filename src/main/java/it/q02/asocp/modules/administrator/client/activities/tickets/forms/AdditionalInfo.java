package it.q02.asocp.modules.administrator.client.activities.tickets.forms;

import com.google.gwt.core.client.GWT;
import com.google.gwt.dom.client.DivElement;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.user.client.ui.IsWidget;
import com.google.gwt.user.client.ui.Widget;

/**
 * User: aleksander at  24.03.14, 19:41
 */
public class AdditionalInfo implements IsWidget {
    private final Widget rootElement;

    @Override
    public Widget asWidget() {
        return rootElement;  //To change body of implemented methods use File | Settings | File Templates.
    }

    interface AdditionalInfoUiBinder extends UiBinder<Widget, AdditionalInfo> {
    }

    private static AdditionalInfoUiBinder ourUiBinder = GWT.create(AdditionalInfoUiBinder.class);

    public AdditionalInfo() {
        rootElement = ourUiBinder.createAndBindUi(this);

    }
}