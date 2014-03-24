package it.q02.asocp.modules.base.client.widgets.selection;

import com.google.gwt.core.client.GWT;
import com.google.gwt.event.dom.client.ClickEvent;
import com.google.gwt.event.dom.client.ClickHandler;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.uibinder.client.UiField;
import com.google.gwt.uibinder.client.UiHandler;
import com.google.gwt.user.client.Window;
import com.google.gwt.user.client.ui.HTML;
import com.google.gwt.user.client.ui.IsWidget;
import com.google.gwt.user.client.ui.Widget;
import it.q02.asocp.modules.base.client.data.RoleMap;
import org.gwtbootstrap3.client.ui.*;
import org.gwtbootstrap3.client.ui.constants.ButtonSize;
import org.gwtbootstrap3.client.ui.constants.ButtonType;

import java.util.Collection;

/**
 *
 */
public class SelectionWidget implements IsWidget {
    //region UiBinder
    interface SelectionPageUiBinder extends UiBinder<ListGroup, SelectionWidget> {
    }

    private static SelectionPageUiBinder ourUiBinder = GWT.create(SelectionPageUiBinder.class);
    //endregion

    protected final ListGroup rootWidget;

    public SelectionWidget(Collection<RoleMap> roleMaps) {
        this.rootWidget = ourUiBinder.createAndBindUi(this);



        for(final RoleMap roleMap:roleMaps){
            ListGroupItem litem = new ListGroupItem();
            Button item = new Button();
            litem.add(item);
            item.setText(roleMap.getTitle());

            item.addClickHandler(new ClickHandler() {
                @Override
                public void onClick(ClickEvent clickEvent) {
                    Window.Location.replace(roleMap.getUrl());
                }
            });

            item.setSize(ButtonSize.LARGE);
            item.setType(ButtonType.SUCCESS);
            item.setWidth("500px");
            item.setHeight("80px");
            rootWidget.add(litem);
        }
    }

    @Override
    public Widget asWidget() {
        return rootWidget;
    }
}
