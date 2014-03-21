package it.q02.asocp.modules.selection.client.page;

import com.google.gwt.core.client.GWT;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.user.client.ui.IsWidget;
import com.google.gwt.user.client.ui.Widget;
import org.gwtbootstrap3.client.ui.FluidContainer;
import org.gwtbootstrap3.client.ui.Modal;

/**
 *
 */
public class SelectionPage implements IsWidget {
    //region UiBinder
    interface SelectionPageUiBinder extends UiBinder<FluidContainer, SelectionPage> {
    }

    private static SelectionPageUiBinder ourUiBinder = GWT.create(SelectionPageUiBinder.class);
    //endregion


    private final FluidContainer rootWidget;


    public SelectionPage() {
        this.rootWidget = ourUiBinder.createAndBindUi(this);

    }


    @Override
    public Widget asWidget() {
        return rootWidget;
    }
}
