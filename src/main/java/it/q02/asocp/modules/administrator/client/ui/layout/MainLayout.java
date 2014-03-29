package it.q02.asocp.modules.administrator.client.ui.layout;

import com.google.gwt.core.client.GWT;
import com.google.gwt.dom.client.DivElement;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.uibinder.client.UiField;
import com.google.gwt.user.client.ui.IsWidget;
import com.google.gwt.user.client.ui.Widget;
import it.q02.asocp.modules.administrator.client.ui.menu.MainMenu;
import it.q02.asocp.modules.administrator.client.ui.menu.SideBarNavigation;
import it.q02.asocp.modules.administrator.client.ui.menu.bus.AddMenuHandler;
import org.gwtbootstrap3.client.ui.Column;
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

    public MainMenu getMenu() {
        return topMenu;
    }

    interface MainLayoutUiBinder extends UiBinder<FluidContainer, MainLayout> {
    }

    private static MainLayoutUiBinder ourUiBinder = GWT.create(MainLayoutUiBinder.class);
    @UiField
    protected SideBarNavigation sideBar;
    @UiField
    protected Column container;
    @UiField
    protected MainMenu topMenu;

    public MainLayout() {
        this.rootWidget = ourUiBinder.createAndBindUi(this);
    }

    public SideBarNavigation getSideBar() {
        return sideBar;
    }

    public Column getContainer() {
        return container;
    }
}