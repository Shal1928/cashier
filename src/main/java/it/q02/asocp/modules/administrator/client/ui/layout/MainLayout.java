package it.q02.asocp.modules.administrator.client.ui.layout;

import com.google.gwt.core.client.GWT;
import com.google.gwt.dom.client.DivElement;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.user.client.ui.IsWidget;
import com.google.gwt.user.client.ui.Widget;
import org.gwtbootstrap3.client.ui.FluidContainer;

/**
 * User: aleksander at  18.03.14, 23:48
 */
public class MainLayout implements IsWidget {
    private final FluidContainer rootWidget;

    @Override
    public Widget asWidget() {
        return rootWidget;  //To change body of implemented methods use File | Settings | File Templates.
    }

    interface MainLayoutUiBinder extends UiBinder<FluidContainer, MainLayout> {
    }

    private static MainLayoutUiBinder ourUiBinder = GWT.create(MainLayoutUiBinder.class);

    public MainLayout() {
        this.rootWidget = ourUiBinder.createAndBindUi(this);

    }
}