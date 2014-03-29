package it.q02.asocp.modules.cashier.client;

import com.google.gwt.core.client.EntryPoint;
import com.google.gwt.user.client.ui.*;
import it.q02.asocp.modules.cashier.client.ui.layout.CashierView;

/**
 * User: aleksander at  16.03.14, 18:35
 */
public class Cashier implements EntryPoint {
    public void onModuleLoad() {
        CashierView cashierView = new CashierView();
        RootPanel.get().add(cashierView);
    }
}
