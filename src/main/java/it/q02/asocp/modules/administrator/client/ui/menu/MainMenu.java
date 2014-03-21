package it.q02.asocp.modules.administrator.client.ui.menu;

import com.google.gwt.core.client.GWT;
import com.google.gwt.dom.client.DivElement;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.user.client.ui.IsWidget;
import com.google.gwt.user.client.ui.Widget;
import org.gwtbootstrap3.client.ui.Navbar;

/**
 * User: aleksander at  21.03.14, 9:07
 */
public class MainMenu implements IsWidget {
    private final Navbar menu;

    @Override
    public Widget asWidget() {
        return menu;  //To change body of implemented methods use File | Settings | File Templates.
    }

    interface MainMenuUiBinder extends UiBinder<Navbar, MainMenu> {
    }

    private static MainMenuUiBinder ourUiBinder = GWT.create(MainMenuUiBinder.class);

    public MainMenu() {
        menu = ourUiBinder.createAndBindUi(this);

    }
}