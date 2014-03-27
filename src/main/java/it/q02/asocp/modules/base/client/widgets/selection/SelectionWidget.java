package it.q02.asocp.modules.base.client.widgets.selection;

import com.google.gwt.core.client.GWT;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.user.client.ui.HasVisibility;
import com.google.gwt.user.client.ui.IsWidget;
import com.google.gwt.user.client.ui.Widget;
import it.q02.asocp.modules.base.client.data.RoleMap;
import org.gwtbootstrap3.client.ui.ListItem;
import org.gwtbootstrap3.client.ui.NavPills;
import org.gwtbootstrap3.client.ui.constants.Pull;

import java.util.Collection;

/**
 *
 */
public class SelectionWidget implements IsWidget, HasVisibility {

    //region UiBinder
    interface SelectionPageUiBinder extends UiBinder<NavPills, SelectionWidget> {
    }

    private static SelectionPageUiBinder ourUiBinder = GWT.create(SelectionPageUiBinder.class);
    //endregion

    protected final NavPills rootWidget;

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
            ListItem item = new ListItem();
            item.setText(roleMap.getTitle());
            item.setPull(Pull.NONE);
            item.setHref(roleMap.getUrl());
            rootWidget.add(item);
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
