package it.q02.asocp.modules.selection.client;

import com.google.gwt.core.client.EntryPoint;
import com.google.gwt.user.client.ui.RootPanel;
import it.q02.asocp.modules.selection.client.page.SelectionPage;

/**
 *
 */
public class SelectionModule implements EntryPoint {
    public void onModuleLoad() {
        RootPanel.get().add(new SelectionPage());
    }
}
