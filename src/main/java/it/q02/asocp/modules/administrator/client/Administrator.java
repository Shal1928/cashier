package it.q02.asocp.modules.administrator.client;

import com.google.gwt.core.client.EntryPoint;
import com.google.gwt.user.client.Window;
import com.google.gwt.user.client.ui.*;
import it.q02.asocp.modules.administrator.client.ui.layout.MainLayout;

/**
 * User: aleksander at  16.03.14, 18:34
 */
public class Administrator implements EntryPoint {
    public void onModuleLoad() {
        Window.alert("Xren!");
        RootPanel.get().add(new MainLayout());

    }
}
