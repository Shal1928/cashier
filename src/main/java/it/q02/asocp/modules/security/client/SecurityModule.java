package it.q02.asocp.modules.security.client;

import com.google.gwt.core.client.EntryPoint;
import com.google.gwt.user.client.Window;
import com.google.gwt.user.client.ui.*;
import it.q02.asocp.modules.security.client.page.LoginPage;

/**
 * User: aleksander at  16.03.14, 18:33
 */
public class SecurityModule implements EntryPoint {
    public void onModuleLoad() {
        new LoginPage();
    }
}
