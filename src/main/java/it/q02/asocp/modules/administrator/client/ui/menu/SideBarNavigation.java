package it.q02.asocp.modules.administrator.client.ui.menu;

import com.google.gwt.core.client.GWT;
import com.google.gwt.dom.client.DivElement;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.user.client.ui.IsWidget;
import com.google.gwt.user.client.ui.Widget;
import org.gwtbootstrap3.client.ui.Nav;

/**
 * User: aleksander at  21.03.14, 9:12
 */
public class SideBarNavigation implements IsWidget {
    private final Nav rootWidget;

    @Override
    public Widget asWidget() {
        return rootWidget;
    }

    interface SideBarNavigationUiBinder extends UiBinder<Nav, SideBarNavigation> {
    }

    private static SideBarNavigationUiBinder ourUiBinder = GWT.create(SideBarNavigationUiBinder.class);

    public SideBarNavigation() {
        rootWidget = ourUiBinder.createAndBindUi(this);

    }
}