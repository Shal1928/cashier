package it.q02.asocp.modules.base.client.widgets.selection;

import com.google.gwt.core.client.GWT;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.user.client.ui.IsWidget;
import com.google.gwt.user.client.ui.Widget;
import org.gwtbootstrap3.client.ui.FluidContainer;

/**
 *
 */
public class SelectionWidget implements IsWidget {
    //region UiBinder
    interface SelectionPageUiBinder extends UiBinder<FluidContainer, SelectionWidget> {
    }

    private static SelectionPageUiBinder ourUiBinder = GWT.create(SelectionPageUiBinder.class);
    //endregion


    private final FluidContainer rootWidget;


    public SelectionWidget(String elements) {
        this.rootWidget = ourUiBinder.createAndBindUi(this);

    }


    @Override
    public Widget asWidget() {
        return rootWidget;
    }
}
