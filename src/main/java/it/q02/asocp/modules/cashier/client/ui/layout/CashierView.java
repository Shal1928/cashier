package it.q02.asocp.modules.cashier.client.ui.layout;

import com.google.gwt.core.client.GWT;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.uibinder.client.UiField;
import com.google.gwt.user.client.ui.IsWidget;
import com.google.gwt.user.client.ui.Widget;
import it.q02.asocp.modules.cashier.client.ui.resources.Resources;
import org.gwtbootstrap3.client.ui.FluidContainer;

/**
 *
 */
public class CashierView implements IsWidget {
    //region UiBinder base
    interface CashierViewUiBinder extends UiBinder<FluidContainer, CashierView> {
    }

    private static CashierViewUiBinder ourUiBinder = GWT.create(CashierViewUiBinder.class);
    //endregion


    //region UiFields
    private FluidContainer rootElement;

    @UiField(provided = true)
    protected Resources res;
    //endregion


    public CashierView() {
        res = GWT.create(Resources.class);
        res.layoutStyle().ensureInjected();
        rootElement = ourUiBinder.createAndBindUi(this);
    }


    //region IsWidget
    @Override
    public Widget asWidget() {
        return rootElement;
    }
    //endregion
}