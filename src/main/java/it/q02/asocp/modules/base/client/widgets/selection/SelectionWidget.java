package it.q02.asocp.modules.base.client.widgets.selection;

import com.github.gwtbootstrap.client.ui.NavLink;
import com.github.gwtbootstrap.client.ui.NavList;

import com.google.gwt.core.client.GWT;
import com.google.gwt.event.dom.client.ClickEvent;
import com.google.gwt.event.dom.client.ClickHandler;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.user.client.Window;
import com.google.gwt.user.client.ui.HasVisibility;
import com.google.gwt.user.client.ui.IsWidget;
import com.google.gwt.user.client.ui.Widget;
import it.q02.asocp.modules.base.client.data.RoleMap;

import java.util.Collection;

/**
 *
 */
public class SelectionWidget implements IsWidget, HasVisibility {

    //region UiBinder
    interface SelectionPageUiBinder extends UiBinder<NavList, SelectionWidget> {
    }

    private static SelectionPageUiBinder ourUiBinder = GWT.create(SelectionPageUiBinder.class);
    //endregion

    protected final NavList rootWidget;

    public void setRoleMaps(Collection<RoleMap> roleMaps) {
        this.roleMaps = roleMaps;
        buildElements(roleMaps);
    }

    private Collection<RoleMap> roleMaps;

    public SelectionWidget() {
        this.rootWidget = ourUiBinder.createAndBindUi(this);
    }

    @Override
    public Widget asWidget() {
        return rootWidget;
    }

    private void buildElements(Collection<RoleMap> roleMaps) {
        rootWidget.clear();
        for (final RoleMap roleMap : roleMaps) {
            NavLink navLink = new NavLink();
            navLink.setText(roleMap.getTitle());
            navLink.addClickHandler(new ClickHandler() {
                @Override
                public void onClick(ClickEvent clickEvent) {
                    Window.Location.replace(roleMap.getUrl());
                }
            });
            rootWidget.add(navLink);
        }
    }

    @Override
    public boolean isVisible() {
        return rootWidget.isVisible();
    }

    @Override
    public void setVisible(boolean b) {
        rootWidget.setVisible(b);
    }
}
