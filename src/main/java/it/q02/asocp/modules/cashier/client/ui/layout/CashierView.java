package it.q02.asocp.modules.cashier.client.ui.layout;

import com.google.gwt.core.client.GWT;
import com.google.gwt.dom.client.DivElement;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.user.client.ui.IsWidget;
import com.google.gwt.user.client.ui.Widget;
import org.gwtbootstrap3.client.ui.FluidContainer;

/**
 *
 */
public class CashierView implements IsWidget {
    interface CashierViewUiBinder extends UiBinder<FluidContainer, CashierView> {
    }

    private static CashierViewUiBinder ourUiBinder = GWT.create(CashierViewUiBinder.class);

    private FluidContainer rootElement = ourUiBinder.createAndBindUi(this);

    public CashierView() {

    }

    @Override
    public Widget asWidget() {
        return rootElement;
    }
}